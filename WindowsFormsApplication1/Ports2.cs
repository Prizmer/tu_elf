using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApplication1
{
    public class SOVirtualPortEmu : Helpers
    {
        public SOVirtualPortEmu(SerialPort mPort)
        {
            this.m_port = mPort;
        }

        SerialPort m_port;
        private ushort m_read_timeout = 1000;

        public delegate int FindPacketSignature(Queue<byte> queue);

        private bool OpenPort()
        {
            try
            {
                if (m_port != null && !m_port.IsOpen)
                    m_port.Open();

                return m_port.IsOpen;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ClosePort()
        {
            if (m_port != null && m_port.IsOpen)
                m_port.Close();
        }



        /// <summary>
        /// Передает даннные из буфера-на-выход в порт, и принимает данные в буфер-на-вход
        /// </summary>
        /// <param name="func"></param>
        /// <param name="out_buffer">Массив байт передаваемый устройству</param>
        /// <param name="in_buffer">Массив байт принимаемый от устройства</param>
        /// <param name="out_length">Определяет длину полезных байт в буфере-на-выход (длину команды)</param>
        /// <param name="target_in_length">Определяет длину полезных байт в буфере-на-вход (длину ответа)</param>
        /// <param name="pos_count_data_size"></param>
        /// <param name="size_data"></param>
        /// <param name="header_size"></param>
        /// <returns></returns>
        public int WriteReadData2(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length,
            uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            int reading_size = 0;

            if (OpenPort())
            {
                Queue<byte> reading_queue = new Queue<byte>(8192);

                try
                {
                    //пишем в порт команду, ограниченную out_length
                    m_port.Write(out_buffer, 0, out_length);


                    //ожидаем 100мс
                    /*TODO: необходимо явную константу заменить m_write_timeout и не учитывать 
                    сделать elapsed_time_count = 0 */
                    Thread.Sleep(100);

                    uint elapsed_time_count = 100;

                    while (elapsed_time_count <= m_read_timeout)
                    {
                        //WriteToLog("- new circle");
                        if (m_port.BytesToRead > 0)
                        {
                            try
                            {
                                byte[] tmp_buff = new byte[m_port.BytesToRead];
                                //WriteToLog("m_Port.BytesToRead: " + m_Port.BytesToRead.ToString());
                                //читаем все данные из входного буфера
                                int readed_bytes = m_port.Read(tmp_buff, 0, tmp_buff.Length);
                                //определяем из в очередь
                                for (int i = 0; i < readed_bytes; i++)
                                    reading_queue.Enqueue(tmp_buff[i]);
                            }
                            catch (Exception ex)
                            {
                                WriteToLog("WriteReadData: " + ex.Message);
                            }

                            //WriteToLog("Request: " + BitConverter.ToString(out_buffer));

                            /*TODO: Откуда взялась константа 4, почему 4?*/
                            if (reading_queue.Count >= 4)
                            {
                                /*попытаемся определить начало полезных данных в буфере-на-вход
                                 при помощи связанного делегата*/
                                int pos = func(reading_queue);
                                if (pos >= 0)
                                {
                                    //избавимся от лишних данных спереди
                                    for (int i = 0; i < pos; i++)
                                    {
                                        reading_queue.Dequeue();
                                    }

                                    //оставшиеся данные преобразуем обратно в массив
                                    byte[] temp_buffer = new byte[reading_size = reading_queue.Count];

                                    //WriteToLog("reading_queue.Count: " + reading_size.ToString());

                                    temp_buffer = reading_queue.ToArray();
                                    //WriteToLog(BitConverter.ToString(temp_buffer));

                                    //если длина полезных данных ответа определена как 0, произведем расчет по необязательнм параметрам
                                    if (target_in_length == 0)
                                    {
                                        if (reading_size > pos_count_data_size)
                                        {
                                            target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);
                                        }
                                    }

                                    if (target_in_length > 0 && reading_size >= target_in_length)
                                    {
                                        reading_size = target_in_length;
                                        //WriteToLog("target_in_length: " + target_in_length.ToString() + "; in_buffer: " + in_buffer.Length); 
                                        for (int i = 0; i < target_in_length && i < in_buffer.Length; i++)
                                        {
                                            in_buffer[i] = temp_buffer[i];
                                        }

                                        break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            //reading_size = 0;
                        }

                        elapsed_time_count += 100;
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog("WriteReadData: " + ex.Message);
                    return -1;
                }
                finally
                {
                    reading_queue.Clear();

                    ClosePort();
                }
            }

            return reading_size;
        }

        public int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length,
    uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            int reading_size = 0;

            if (OpenPort())
            {
                //очередь для поддержки делегатов в старых драйверах
                Queue<byte> reading_queue = new Queue<byte>(8192);
                List<byte> readBytesList = new List<byte>(8192);
               
                try
                {
                    //пишем в порт команду, ограниченную out_length
                    m_port.Write(out_buffer, 0, out_length);
                }
                catch (Exception ex)
                {
                    WriteToLog("WriteReadData: Write to port error: " + ex.Message);
                }


               Thread.Sleep(100);
               uint elapsed_time_count = 100;

               while (elapsed_time_count <= m_read_timeout)
               {
                    if (m_port.BytesToRead > 0)
                    {
                        try
                        {
                            byte[] tmp_buff = new byte[m_port.BytesToRead];
                            int readed_bytes = m_port.Read(tmp_buff, 0, tmp_buff.Length);

                            readBytesList.AddRange(tmp_buff);
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("WriteReadData: Read from port error: " + ex.Message);
                        }
                    }

                    elapsed_time_count += 100;
                    Thread.Sleep(100);
                }


                /*TODO: Откуда взялась константа 4, почему 4?*/
                if (readBytesList.Count >= 4)
                {
                    /*попытаемся определить начало полезных данных в буфере-на-вход
                        при помощи связанного делегата*/
                    for (int i = 0; i < readBytesList.Count; i++)
                        reading_queue.Enqueue(readBytesList[i]);

                    int pos = func(reading_queue);
                    if (pos >= 0)
                    {
                        //избавимся от лишних данных спереди
                        for (int i = 0; i < pos; i++)
                        {
                            reading_queue.Dequeue();
                        }

                        //оставшиеся данные преобразуем обратно в массив
                        byte[] temp_buffer = new byte[reading_size = reading_queue.Count];

                        //WriteToLog("reading_queue.Count: " + reading_size.ToString());

                        temp_buffer = reading_queue.ToArray();
                        //WriteToLog(BitConverter.ToString(temp_buffer));

                        //если длина полезных данных ответа определена как 0, произведем расчет по необязательнм параметрам
                        if (target_in_length == 0)
                        {
                            if (reading_size > pos_count_data_size)
                                target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);
                        }

                        if (target_in_length == -1)
                        {
                            target_in_length = reading_queue.Count;
                            reading_size = target_in_length;
                            in_buffer = new byte[reading_size];

                            for (int i = 0; i < in_buffer.Length; i++)
                                in_buffer[i] = temp_buffer[i];

                            return reading_size;
                        }

                        if (target_in_length > 0 && reading_size >= target_in_length)
                        {
                            reading_size = target_in_length;                                    
                            for (int i = 0; i < target_in_length && i < in_buffer.Length; i++)
                            {
                                in_buffer[i] = temp_buffer[i];
                            }
                        }
                    }
                }
            }
            else
            {
                WriteToLog("Open port Error");
            }

            return reading_size;
        }

    }

    public class SerialPortManager : Helpers
    {
        public SerialPortManager(SerialPort m_port)
        {
            this.m_port = m_port;
            m_port.DataReceived += new SerialDataReceivedEventHandler(m_port_DataReceived);
        }

        private bool data_ready = false;
        private void m_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data_ready = true;
        }

        private SerialPort m_port = null;

        private bool WriteData(byte[] out_byte_arr)
        {
            try
            {
                m_port.Write(out_byte_arr, 0, out_byte_arr.Length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ReadData(ref byte[] inp_data_arr)
        {
            int bytes_to_read = m_port.BytesToRead;
            WriteToLog("ReadData: bytes to read: " + bytes_to_read.ToString());
            if (bytes_to_read == 0) return false;

            inp_data_arr = new byte[bytes_to_read];

            try
            {
                m_port.Read(inp_data_arr, 0, inp_data_arr.Length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int write_read_timeout = 1000;
        public int WriteReadTimeout
        {
            get { return write_read_timeout; }
            set { write_read_timeout = value; }
        }

        public bool WriteReadData(byte[] out_byte_arr, ref byte[] inp_data_arr)
        {
            WriteToLog("WriteReadData- start"); 
            if (OpenPort())
            {
                WriteToLog("WriteReadData- portisopened"); 
                int wr_r_t = write_read_timeout;
                WriteData(out_byte_arr);
                WriteToLog("WriteReadData- data is written to port"); 
                List<byte> inp_data_list = new List<byte>(200);

                while (wr_r_t > 0)
                {
                    WriteToLog("WriteReadData: " + wr_r_t.ToString());
                    WriteToLog("WriteReadData_dataready: " + data_ready.ToString());

                    if (!data_ready || !ReadData(ref inp_data_arr))
                    {
                        Thread.Sleep(100);
                        wr_r_t -= 100;
                    }
                    else
                    {
                        inp_data_list.AddRange(inp_data_arr);
                        Thread.Sleep(100);
                        wr_r_t -= 100;
                    }
                }

                data_ready = false;

                //потому что минимальная команда 7 байт
                if (inp_data_list.Count > 7)
                {
                    inp_data_arr = inp_data_list.ToArray();
                    ClosePort();
                    return true;
                }
                else
                {
                    inp_data_arr = null;
                    ClosePort();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool OpenPort()
        {
            try
            {
                if (m_port != null && !m_port.IsOpen)
                    m_port.Open();

                return m_port.IsOpen;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ClosePort()
        {
            if (m_port != null && m_port.IsOpen)
                m_port.Close();
        }
    }
}
