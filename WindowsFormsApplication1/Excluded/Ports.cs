﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using System.Configuration;

namespace Prizmer.Ports
{
    public delegate int FindPacketSignature(Queue<byte> queue);

    public interface VirtualPort
    {
        int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0);
        string GetName();
        bool isOpened();
        SerialPort getSerialPortObject();

        bool OpenPort();
        void ClosePort();
    }

    public sealed class TcpipPort : VirtualPort
    {
        private string m_address = "127.0.0.1";
        private int m_port = 80;
        private ushort m_write_timeout = 100;
        private ushort m_read_timeout = 800;
        private int m_delay_between_sending = 100;

        public string GetName()
        {
            return "tcp/ip " + m_address + ":" + m_port + " ";
        }

        public TcpipPort(string address, int port, ushort write_timeout, ushort read_timeout, int delay_between_sending)
        {
            m_address = address;
            m_port = port;
            m_write_timeout = write_timeout;
            m_read_timeout = read_timeout;
            m_delay_between_sending = delay_between_sending;
        }

        public void Write(byte[] m_cmd, int leng)
        {

        }

        public int Read(ref byte[] data)
        {
            return 0;
        }

        public int WriteReadData2(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            int reading_size = 0;
            using (TcpClient tcp = new TcpClient())
            {
                Queue<byte> reading_queue = new Queue<byte>(8192);

                try
                {
                    Thread.Sleep(m_delay_between_sending);

                    IAsyncResult ar = tcp.BeginConnect(m_address, m_port, null, null);
                    using (WaitHandle wh = ar.AsyncWaitHandle)
                    {
                        if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10), false))
                        {
                            throw new TimeoutException();
                        }
                        else
                        {
                            if (tcp.Client.Connected)
                            {
                                tcp.Client.ReceiveTimeout = m_read_timeout;
                                tcp.Client.SendTimeout = m_write_timeout;

                                // посылка данных
                                if (tcp.Client.Send(out_buffer, out_length, SocketFlags.None) == out_length)
                                {
                                    uint elapsed_time_count = 0;

                                    Thread.Sleep(50);

                                    // чтение данных
                                    while (elapsed_time_count < m_read_timeout)
                                    {
                                        if (tcp.Client.Available > 0)
                                        {
                                            try
                                            {
                                                byte[] tmp_buff = new byte[tcp.Available];
                                                int readed_bytes = tcp.Client.Receive(tmp_buff, 0, tmp_buff.Length, SocketFlags.None);

                                                for (int i = 0; i < readed_bytes; i++)
                                                {
                                                    reading_queue.Enqueue(tmp_buff[i]);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteToLog("Receive: " + ex.Message);
                                            }
                                        }
                                        elapsed_time_count += 50;
                                        Thread.Sleep(50);
                                    }

                                    int pos = -1;
                                    if ((pos = func(reading_queue)) >= 0)
                                    {
                                        for (int i = 0; i < pos; i++)
                                        {
                                            reading_queue.Dequeue();
                                        }

                                        byte[] temp_buffer = new byte[reading_size = reading_queue.Count];
                                        temp_buffer = reading_queue.ToArray();

                                        if (target_in_length == 0)
                                        {
                                            if (reading_size > pos_count_data_size)
                                            {
                                                target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);
                                            }
                                        }

                                        if (target_in_length > 0)
                                        {
                                            if (reading_size >= target_in_length)
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
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(ex.Message);
                    return -1;
                }
                finally
                {
                    reading_queue.Clear();
                }
            }

            return reading_size;
        }

        public int WriteReadData3(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];
            List<byte> receivedBytes = new List<byte>();

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                IPAddress ipaddr = IPAddress.Parse(m_address.ToString());
                IPEndPoint remoteEP = new IPEndPoint(ipaddr, m_port);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                sender.ReceiveTimeout = m_read_timeout;

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    WriteToLog(String.Format("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString()));

                    // Send the data through the socket.
                    int bytesSent = sender.Send(out_buffer);

                    while (true)
                    {
                        bytes = new byte[1024];
                        try
                        {
                            // Receive the response from the remote device.
                            int bytesRec = sender.Receive(bytes);

                            for (int i = 0; i < bytesRec; i++)
                                receivedBytes.Add(bytes[i]);
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("Timeout");
                            break;
                        }
                    }

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    WriteToLog("Disconected from server");
                }
                catch (Exception exx)
                {
                    WriteToLog("Connection exception: " + exx.Message);
                }

            }
            catch (Exception e)
            {
                WriteToLog("Common exception: " + e.Message);
            }

            WriteToLog("Totally bytes received: " + BitConverter.ToString(receivedBytes.ToArray()).Replace("-", " "));

            in_buffer = receivedBytes.ToArray();
            return receivedBytes.Count;
        }


        public int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {

            //return WriteReadData(out_buffer, ref in_buffer);

            int reading_size = 0;
            using (TcpClient tcp = new TcpClient())
            {
                Queue<byte> reading_queue = new Queue<byte>(8192);

                try
                {
                    Thread.Sleep(m_delay_between_sending);

                    IAsyncResult ar = tcp.BeginConnect(m_address, m_port, null, null);
                    using (WaitHandle wh = ar.AsyncWaitHandle)
                    {
                        if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10), false))
                        {
                            throw new TimeoutException();
                        }
                        else
                        {
                            if (tcp.Client.Connected)
                            {
                                tcp.Client.ReceiveTimeout = m_read_timeout;
                                tcp.Client.SendTimeout = m_write_timeout;

                                // посылка данных
                                if (tcp.Client.Send(out_buffer, out_length, SocketFlags.None) == out_length)
                                {
                                    uint elapsed_time_count = 0;

                                    Thread.Sleep(50);

                                    // чтение данных
                                    while (elapsed_time_count < m_read_timeout)
                                    {
                                        if (tcp.Client.Available > 0)
                                        {
                                            try
                                            {
                                                byte[] tmp_buff = new byte[tcp.Available];
                                                int readed_bytes = tcp.Client.Receive(tmp_buff, 0, tmp_buff.Length, SocketFlags.None);

                                                for (int i = 0; i < readed_bytes; i++)
                                                {
                                                    reading_queue.Enqueue(tmp_buff[i]);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteToLog("Receive: " + ex.Message);
                                            }
                                        }
                                        elapsed_time_count += 50;
                                        Thread.Sleep(50);
                                    }



                                    int pos = -1;
                                    if ((pos = func(reading_queue)) >= 0)
                                    {
                                        for (int i = 0; i < pos; i++)
                                        {
                                            reading_queue.Dequeue();
                                        }

                                        byte[] temp_buffer = new byte[reading_size = reading_queue.Count];
                                        temp_buffer = reading_queue.ToArray();

                                        if (target_in_length == 0)
                                        {
                                            if (reading_size > pos_count_data_size)
                                            {
                                                target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);
                                            }
                                        }

                                        if (target_in_length > 0)
                                        {
                                            if (reading_size >= target_in_length)
                                            {
                                                reading_size = target_in_length;
                                                for (int i = 0; i < target_in_length && i < in_buffer.Length; i++)
                                                {
                                                    in_buffer[i] = temp_buffer[i];
                                                }
                                            }
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

                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(ex.Message);
                    return -1;
                }
                finally
                {
                    reading_queue.Clear();
                }
            }

            return reading_size;
        }

        public void WriteToLog(string str)
        {
            if (false)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("TcpPorts.log", true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + ": " + GetName() + ": " + str);
                    }
                }
                catch
                {
                }

            }

        }

        public bool isOpened()
        {
            return true;
        }


        public SerialPort getSerialPortObject()
        {
            throw new NotImplementedException();
        }


        public bool OpenPort()
        {
            throw new NotImplementedException();
        }

        public void ClosePort()
        {
            //
        }
    }

    public sealed class ComPort : VirtualPort, IDisposable
    {
        private SerialPort m_Port;
        private string m_name = "COM1";
        private int m_baudrate = 9600;
        private int m_data_bits = 8;
        private Parity m_parity = Parity.None;
        private StopBits m_stop_bits = StopBits.One;
        private ushort m_write_timeout = 500;
        private ushort m_read_timeout = 1000;
        private byte m_attemts = 1;

        public string GetName()
        {
            return m_name + " ";
        }

        public ComPort(byte number, int baudrate, byte data_bits, byte parity, byte stop_bits, ushort write_timeout, ushort read_timeout, byte attemts)
        {
            m_name = "COM" + Convert.ToString(number);
            m_baudrate = baudrate;
            m_data_bits = data_bits;
            m_parity = (Parity)parity;
            m_stop_bits = (StopBits)stop_bits;
            m_write_timeout = write_timeout;
            m_read_timeout = read_timeout;
            m_attemts = attemts;

            try
            {
                m_Port = new SerialPort(m_name, m_baudrate, m_parity, m_data_bits, m_stop_bits);

                /*ELF: для работы с elf108*/
                m_Port.DtrEnable = true;
                m_Port.RtsEnable = true;
            }
            catch (Exception ex)
            {
#if (DEBUG)
                WriteToLog("Create " + m_name + ": " + ex.Message);
#endif
            }
        }

        public ComPort(SerialPort sp, byte attempts, ushort read_timout, ushort write_timeout)
        {

            m_attemts = attempts;
            m_name = sp.PortName;
            m_baudrate = sp.BaudRate;
            m_data_bits = sp.DataBits;
            m_parity = sp.Parity;
            m_stop_bits = sp.StopBits;
            m_read_timeout = read_timout;
            m_write_timeout = write_timeout;



            try
            {
                m_Port = new SerialPort(m_name, m_baudrate, m_parity, m_data_bits, m_stop_bits);
                m_Port.WriteTimeout = m_write_timeout;
                m_Port.ReadTimeout = m_read_timeout;

                /*ELF: для работы с elf108*/
                m_Port.DtrEnable = sp.DtrEnable;
                //m_Port.RtsEnable = true;
            }
            catch (Exception ex)
            {

            }
        }


        public void Dispose()
        {
            if (m_Port != null)
            {
                m_Port.Dispose();
            }
        }

        public bool OpenPort()
        {
            try
            {
                if (m_Port != null)
                {
                    if (!m_Port.IsOpen)
                    {
                        m_Port.Open();
                    }
                }

                return m_Port.IsOpen;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ClosePort()
        {
            try
            {
                if (m_Port != null)
                {
                    if (m_Port.IsOpen)
                    {
                        m_Port.Close();
                        //m_Port = null;
                    }
                }
            }
            catch (Exception ex)
            {
#if (DEBUG)
                WriteToLog("Close " + m_name + ": " + ex.Message);
#endif
            }
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
                    m_Port.Write(out_buffer, 0, out_length);
                }
                catch (Exception ex)
                {
                    WriteToLog("WriteReadData: Write to port error: " + ex.Message);
                }


                Thread.Sleep(100);
                uint elapsed_time_count = 100;

                while (elapsed_time_count <= m_read_timeout)
                {
                    if (m_Port.BytesToRead > 0)
                    {
                        try
                        {
                            byte[] tmp_buff = new byte[m_Port.BytesToRead];
                            int readed_bytes = m_Port.Read(tmp_buff, 0, tmp_buff.Length);

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

                WriteToLog("WriteReadData: received bytes: " + BitConverter.ToString(readBytesList.ToArray(), 0).Replace('-', ' '));

                /*TODO: Откуда взялась константа 4, почему 4?*/
                if (readBytesList.Count > 0)
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


                            ClosePort();
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

            ClosePort();
            return reading_size;
        }

        public void WriteToLog(string str)
        {
            if (false)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("ComPorts.log", true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + ": " + GetName() + ": " + str);
                    }
                }
                catch
                {
                }
            }
        }


        public bool isOpened()
        {
            return m_Port.IsOpen;
        }

        public SerialPort getSerialPortObject()
        {
            return this.m_Port;
        }
    }
}

namespace Prizmer.Ports2222222222
{
    public delegate int FindPacketSignature(Queue<byte> queue);

    public interface VirtualPort
    {
        int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0);
        string GetName();

        string GetConnectionType();
        bool ReInitialize();
        bool GetLocalEndPoint(ref IPEndPoint localEp);
    }

    public sealed class TcpipPort : VirtualPort
    {
        private string m_address = "127.0.0.1";
        private int m_port = 80;
        private ushort m_write_timeout = 100;
        private ushort m_read_timeout = 100;
        private int m_delay_between_sending = 100;

        public string GetName()
        {
            return "tcp" + m_address + "_" + m_port;
        }

        ~TcpipPort()
        {
            if (sender != null)
                sender.Close();
        }
        
        IPAddress ipLocalAddr = null;
        IPEndPoint ipLocalEndpoint = null;
        IPEndPoint remoteEndPoint = null;
        Socket sender = null;

        DateTime dtCreated = DateTime.Now;

        public TcpipPort(string address, int port, ushort write_timeout, ushort read_timeout, int delay_between_sending)
        {
            m_address = address;
            m_port = port;
            m_write_timeout = write_timeout;
            m_read_timeout = read_timeout;
            m_delay_between_sending = delay_between_sending;

            dtCreated = DateTime.Now;
            ReInitialize();
        }


        public bool ReInitialize()
        {
            try
            {
                if (sender != null) sender.Close();

                byte[] ipAddrLocalArr = { 192, 168, 0, 1 };
                ipLocalAddr = new IPAddress(ipAddrLocalArr);

                bool bRes = GetLocalEndPointIp(ref ipLocalAddr);
                ipLocalEndpoint = new IPEndPoint(ipLocalAddr, GetFreeTcpPort());
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(m_address), (int)m_port);

                sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sender.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                LingerOption lingOpt = new LingerOption(true, 0);
                sender.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingOpt);

                sender.ReceiveTimeout = 800;
                sender.SendTimeout = 400;

                try
                {
                    WriteToLog("IpLocalEndp: " + ipLocalEndpoint.ToString() + ";  Remote: " + remoteEndPoint.ToString() );

                    sender.Bind(ipLocalEndpoint);
                    sender.Connect(remoteEndPoint);

                    return true;
                }
                catch (Exception ex)
                {
                    WriteToLog("При создании потока для tcp порта, порт не был инициализирован по причине: " + ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToLog("TCP порт не инициализирован по причине: " + ex.Message);
                return false;
            }
        }


        bool GetTCPPortLiveMinutes(out int timeout)
        {
            timeout = 60;

            string tmpValStr = "";
            try
            {
                tmpValStr = ConfigurationManager.AppSettings.GetValues("tcpPortLiveMinutes")[0];
            }
            catch (Exception ex)
            {
                return false;
            }

            bool parsingResult = false;
            if (tmpValStr.Length > 0)
                parsingResult = int.TryParse(tmpValStr, out timeout);

            if (parsingResult)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int GetFreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
        bool IsTcpPortFree(int tcpPort)
        {
            bool isAvailable = true;

            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == tcpPort)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ip.ToString().StartsWith("192.168"))
                        return ip.ToString();
                }
            }

            return "192.168.0.1";
        }
        bool GetLocalEndPointIp(ref IPAddress localEndpointIp)
        {
            string strIpConfig = "";
            try
            {
                strIpConfig = ConfigurationManager.AppSettings.GetValues("localEndPointIp")[0];
            }
            catch (Exception ex)
            { }

            bool parsingResult = false;
            if (strIpConfig.Length > 0)
                parsingResult = IPAddress.TryParse(strIpConfig, out localEndpointIp);

            if (parsingResult)
            {
                return true;
            }
            else
            {
                strIpConfig = GetLocalIPAddress();
                return parsingResult = IPAddress.TryParse(strIpConfig, out localEndpointIp);
            }
        }

        public bool ManageUpWithReceivedBytes(List<byte> readBytesList, 
            FindPacketSignature func,  int target_in_length,
            out byte[] outDataArr, out int outReadingSize,
            uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            outDataArr = new byte[readBytesList.Count];
            outDataArr[0] = 0x0;
            outReadingSize = 0;

            //очередь для поддержки делегатов в старых драйверах
            Queue<byte> reading_queue = new Queue<byte>(8192);

            int reading_size = 0;

            if (readBytesList.Count > 0)
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
                        reading_queue.Dequeue();

                    //оставшиеся данные преобразуем обратно в массив
                    byte[] temp_buffer = new byte[reading_size = reading_queue.Count];

                    temp_buffer = reading_queue.ToArray();
                    //WriteToLog(BitConverter.ToString(temp_buffer));

                    //если длина полезных данных ответа определена как 0, произведем расчет по необязательнм параметрам
                    if (target_in_length == 0)
                    {
                        if (reading_size > pos_count_data_size)
                            target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);

                        outReadingSize = reading_size;
                        return true;
                    }

                    if (target_in_length == -1)
                    {
                            target_in_length = reading_queue.Count;
                            reading_size = target_in_length;
                            outDataArr = new byte[reading_size];

                            for (int i = 0; i < outDataArr.Length; i++)
                                outDataArr[i] = temp_buffer[i];

                            outReadingSize = reading_size;
                            return true;
                    }

                    if (target_in_length > 0 && reading_size >= target_in_length)
                    {
                        outDataArr = new byte[target_in_length];
                        reading_size = target_in_length;
                        for (int i = 0; i < target_in_length && i < outDataArr.Length; i++)
                            outDataArr[i] = temp_buffer[i];

                        outReadingSize = reading_size;
                        return true;
                    }
                }
            }

            return false;
        }

        public int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length, uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            TimeSpan ts = DateTime.Now - dtCreated;
            int tcpAliveMinutes = 60;
            GetTCPPortLiveMinutes(out tcpAliveMinutes);
            if (ts.TotalMinutes >= tcpAliveMinutes)
            {
                //погружаемся в сон на 5 минут, чтобы "дать отдохнуть" принимающим устройствам
                //WriteToLog("WriteReadData: погружаемся в сон на 5 секунд, чтобы дать отдохнуть принимающим устройствам");
                if (sender != null && sender.Connected) sender.Close();
                Thread.Sleep(1000 * 5);
                dtCreated = DateTime.Now;

                ReInitialize();
                //WriteToLog("WriteReadData: открыт новый сокет после сна: " + sender.LocalEndPoint.ToString());
            }

            List<byte> readBytesList = new List<byte>(8192);
            int readingSize = 0;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    if (sender.Connected)
                    {
                        // Send the data through the socket.
                        sender.Send(out_buffer, 0, out_length, SocketFlags.None);

                        Thread.Sleep(10);
                        uint elapsed_time_count = 100;

                        while (elapsed_time_count <= m_read_timeout)
                        {
                            if (sender.Available > 0)
                            {
                                try
                                {
                                    byte[] tmp_buff = new byte[sender.Available];
                                    int readed_bytes = sender.Receive(tmp_buff);

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

                        //WriteToLog("WriteReadData: before manageupWithBytes, readBytesList.Count: " + readBytesList.Count);

                        bool bManageRes = ManageUpWithReceivedBytes(readBytesList, func, target_in_length, out in_buffer, out readingSize,
                            pos_count_data_size, size_data, header_size);

                        if (bManageRes) break;
                    }
                    else
                    {
                        if (i == 0)
                            ReInitialize();
                        else
                            WriteToLog("WriteReadData: ошибка соединения");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("WriteReadData: " + ex.Message);
                return -1;
            }

            return readingSize;
        }

        public void WriteToLog(string str)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"tcp_ports.log", true, Encoding.Default))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + GetName() + ": " + str);
                }
            }
            catch
            {
            }
        }

        public string GetConnectionType()
        {
            return "tcp";
        }

        public bool GetLocalEndPoint(ref IPEndPoint localEp)
        {
            return false;
        }
    }

    public sealed class ComPort : VirtualPort, IDisposable
    {
        private SerialPort m_Port;
        private string m_name = "COM1";
        private int m_baudrate = 9600;
        private int m_data_bits = 8;
        private Parity m_parity = Parity.None;
        private StopBits m_stop_bits = StopBits.One;
        private ushort m_write_timeout = 100;
        private ushort m_read_timeout = 100;
        private byte m_attemts = 0;

        public string GetName()
        {
            return m_name + " ";
        }

        public ComPort(byte number, int baudrate, byte data_bits, byte parity, byte stop_bits, ushort write_timeout, ushort read_timeout, byte attemts)
        {
            m_name = "COM" + Convert.ToString(number);
            m_baudrate = baudrate;
            m_data_bits = data_bits;
            m_parity = (Parity)parity; 
            m_stop_bits = (StopBits)stop_bits;
            m_write_timeout = write_timeout;
            m_read_timeout = read_timeout;
            m_attemts = attemts;

            try
            {
                m_Port = new SerialPort(m_name, m_baudrate, m_parity, m_data_bits, m_stop_bits);

                /*ELF: для работы с elf108*/
                m_Port.DtrEnable = true;
                m_Port.RtsEnable = true;
            }
            catch (Exception ex)
            {
                #if (DEBUG)
                    WriteToLog("Create " + m_name + ": " + ex.Message);
                #endif
            }
        }

        public void Dispose()
        {
            if (m_Port != null)
            {
                m_Port.Dispose();
            }
        }

        private bool OpenPort()
        {
            try
            {
                if (m_Port != null)
                {
                    if (!m_Port.IsOpen)
                    {
                        m_Port.Open();
                    }
                }

                return m_Port.IsOpen;
            }
            catch (Exception ex)
            {
#if (DEBUG)
                WriteToLog("Open " + m_name + ": " + ex.Message);
#endif
                return false;
            }
        }

        private void ClosePort()
        {
            try
            {
                if (m_Port != null)
                {
                    if (m_Port.IsOpen)
                    {
                        m_Port.Close();
                        //m_Port = null;
                    }
                }
            }
            catch (Exception ex)
            {
#if (DEBUG)
                WriteToLog("Close " + m_name + ": " + ex.Message);
#endif
            }
        }

        #region Старые версии функций
        /*public void Write(byte[] m_cmd, int leng)
        {
            if (OpenPort())
            {
                m_Port.Write(m_cmd, 0, leng);
            }
        }

        public int Read(ref byte[] data)
        {
            int reading_size = 0;

            uint elapsed_time_count = 0;

            Queue<byte> reading_queue = new Queue<byte>(8192);

            Thread.Sleep(100);
            //while (elapsed_time_count < m_read_timeout)
            {
                if (m_Port.BytesToRead > 0)
                {
                    try
                    {
                        byte[] tmp_buff = new byte[m_Port.BytesToRead];
                        reading_size = m_Port.Read(tmp_buff, 0, tmp_buff.Length);

                        data = tmp_buff;

                        
                    }
                    catch (Exception ex)
                    {
                        WriteToLog("ReadData: " + ex.Message);
                    }

                    //WriteToLog("Request: " + BitConverter.ToString(out_buffer));
                }
                //elapsed_time_count += 100;
                //Thread.Sleep(100);
            }

            ClosePort();

            return reading_size;
        }*/

        /*
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
        public int WriteReadData(FindPacketSignature func, byte[] out_buffer, ref byte[] in_buffer, int out_length, int target_in_length,
            uint pos_count_data_size = 0, uint size_data = 0, uint header_size = 0)
        {
            int reading_size = 0;

            if (OpenPort())
            {
                Queue<byte> reading_queue = new Queue<byte>(8192);

                try
                {
                    //пишем в порт команду, ограниченную out_length
                    m_Port.Write(out_buffer, 0, out_length);
                    
                    //ожидаем 100мс
                    Thread.Sleep(100);
                    uint elapsed_time_count = 100;

                    while (elapsed_time_count <= m_read_timeout)
                    {
                        if (m_Port.BytesToRead > 0)
                        {
                            try
                            {
                                byte[] tmp_buff = new byte[m_Port.BytesToRead];
                                //читаем все данные из входного буфера
                                int readed_bytes = m_Port.Read(tmp_buff, 0, tmp_buff.Length);
                                //определяем из в очередь
                                for (int i = 0; i < readed_bytes; i++)
                                    reading_queue.Enqueue(tmp_buff[i]);
                            }
                            catch (Exception ex)
                            {
                                WriteToLog("WriteReadData: " + ex.Message);
                            }

                            //WriteToLog("Request: " + BitConverter.ToString(out_buffer));

                            //TODO: Откуда взялась константа 4, почему 4?
                            if (reading_queue.Count >= 4)
                            {

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
                                    temp_buffer = reading_queue.ToArray();

                                    //если длина полезных данных ответа не
                                    if (target_in_length == 0)
                                    {
                                        if (reading_size > pos_count_data_size)
                                        {
                                            target_in_length = Convert.ToInt32(temp_buffer[pos_count_data_size] * size_data + header_size);
                                        }
                                    }

                                    if (target_in_length > 0)
                                    {
                                        if (reading_size >= target_in_length)
                                        {
                                            reading_size = target_in_length;
                                            for (int i = 0; i < target_in_length && i < in_buffer.Length; i++)
                                            {
                                                in_buffer[i] = temp_buffer[i];
                                            }
                                            break;
                                        }
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
         */
        #endregion

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
                    m_Port.Write(out_buffer, 0, out_length);
                }
                catch (Exception ex)
                {
                    WriteToLog("WriteReadData: Write to port error: " + ex.Message);
                }


                Thread.Sleep(100);
                uint elapsed_time_count = 100;

                while (elapsed_time_count <= m_read_timeout)
                {
                    if (m_Port.BytesToRead > 0)
                    {
                        try
                        {
                            byte[] tmp_buff = new byte[m_Port.BytesToRead];
                            int readed_bytes = m_Port.Read(tmp_buff, 0, tmp_buff.Length);

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
                if (readBytesList.Count > 0)
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

                            ClosePort();
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

            ClosePort();
            return reading_size;
        }

        public void WriteToLog(string str)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"logs\com_ports.log", true, Encoding.Default))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + GetName() + ": " + str);
                }
            }
            catch
            {
            }
        }


        public string GetConnectionType()
        {
            return "com";
        }

        public bool ReInitialize()
        {
            return false;
        }

        public bool GetLocalEndPoint(ref IPEndPoint localEp)
        {
            localEp = null;
            return false;
        }
    }
}