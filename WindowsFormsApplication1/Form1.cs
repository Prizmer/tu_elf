using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;

using System.Collections;
using Prizmer.Ports;
using Prizmer.Meters;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //настройка интерфейса
            List<string> addrLst = new List<string>(256);
            for (int i = 1; i < 256; i++)
                addrLst.Add(Convert.ToString(i, 16));

            List<string> addrLstDec = new List<string>(256);
            for (int i = 1; i < 256; i++)
                addrLstDec.Add(Convert.ToString(i, 10));

            string[] avaluablePorts = SerialPort.GetPortNames();
            portCmbBox.Items.AddRange(avaluablePorts);

            addressCmbBox.Items.AddRange(addrLst.ToArray());
            addressDecCmb.Items.AddRange(addrLstDec.ToArray());

            //экземпляр драйвера
            Meter = new elf108();

            portCmbBox.SelectedIndex = 0;
            addressCmbBox.SelectedIndex = 0;
            radioButton1.Checked = true;

            clearScreen();
            addMessageToScreen("Приветствие!");
            addMessageToScreen("Уточните настройки порта и нажмите \"Применить\"");
            addMessageToScreen();
            addMessageToScreen("Значение \"-1\" следует считать некорректным");
            //Parity.Even.GetHashCode().ToString();

            portInitialized = false;

        }

        elf108 Meter;
        VirtualPort vp;
        byte m_addr;

        private void initCOM(string portName)
        {
            /* Инициализация последовательного порта*/
            /*
            SerialPort m_Port = new SerialPort(portName);
            m_Port.BaudRate = 2400;
            m_Port.DataBits = 8;
            m_Port.Parity = System.IO.Ports.Parity.Even;
            m_Port.StopBits = System.IO.Ports.StopBits.One;

            m_Port.DtrEnable = true;
            m_Port.RtsEnable = true;
             * */
            if (vp != null)
            {
                vp.closePort();
                vp = null;
            }
            vp = new ComPort(byte.Parse(portName.Remove(0, 3)), 2400, 8, 2, 1, 0, (ushort)numericUpDown4.Value, 1);
        }

        private void initTCPIP(string addr, string port)
        {
            vp = new TcpipPort(addr, int.Parse(port), 1000, 1000, 50);
        }

        private void initAddr(string addressHex)
        {
            m_addr = Convert.ToByte(addressHex, 16);
        }

        private void addMessageToScreen(string str = "")
        {
            apatorScreen.Text += str + "\n";
        }
        private void clearScreen()
        {
            apatorScreen.Clear();
        }

        private void encryptorCRC8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpUtilsForm huf = new HelpUtilsForm();
            huf.Show();
        }

        private void apatorBtn_Click(object sender, EventArgs e)
        {
            clearScreen();
            string tmp = "";
            if (Meter.ReadSerialNumber(ref tmp))
            {
                addMessageToScreen("SN: " + tmp);
            }
            else
            {
                addMessageToScreen("Не удалось прочитать серийный номер");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string about = "ООО \"ПРИЗМЕР\"\nМосква 2015 г.";
            MessageBox.Show(about, "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rb_portselect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                if (rb.Tag.ToString() == "rb_com")
                {
                    panel1.Enabled = true;
                    panel2.Enabled = false;
                }
                else if (rb.Tag.ToString() == "rb_tcp")
                {
                    panel2.Enabled = true;
                    panel1.Enabled = false;
                }
            }
        }

        private void portSettingsApplyBtn_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                initCOM(portCmbBox.SelectedItem.ToString());
            }
            else
            {
                initTCPIP(textBox1.Text, textBox2.Text);
            }

            initAddr(addressCmbBox.SelectedItem.ToString());
            Meter.Init(m_addr, String.Empty, vp);


            clearScreen();

            /*
            if (Meter.OpenLinkCanal())
            {
                addMessageToScreen("Метод драйвера OpenLinkCanal вернул FALSE");
            }
            else
            {
                addMessageToScreen("Метод драйвера OpenLinkCanal вернул FALSE");
            }

             * */
            portInitialized = true;
        }

        private void byIdBtn_Click(object sender, EventArgs e)
        {
            
            uint recIndex = (uint)numericUpDown1.Value;
            clearScreen();

            addMessageToScreen("*** Параметры по PT ***");
            addMessageToScreen("Запись с индексом: " + recIndex);
            addMessageToScreen("-----------------------");

            //default settings
            ArchiveValue av = new ArchiveValue();
            av.id = -1;
            av.dt = new DateTime();
            av.energy = -1;
            av.volume = -1;
            av.timeOn = -1;
            av.timeErr = -1;

            if (Meter.ReadArchiveValById(recIndex, ref av)){
                string res = String.Format("ID: {5}\nДата: {4}\nЭнергия: {0}\nОбъем: {1}\nВремя работы: {2}\nВремя с ошибкой: {3}\n",
                    av.energy, av.volume, av.timeOn, av.timeErr, av.dt, av.id);
                addMessageToScreen(res);

            }else{
                addMessageToScreen("Метод драйвера ReadArchiveValById вернул false");
            }
        }

        private void lastRecBtn_Click(object sender, EventArgs e)
        {
            clearScreen();

            addMessageToScreen("*** Параметры по PT ***");
            addMessageToScreen("Чтение последней записи");
            addMessageToScreen("-----------------------");
            //default settings
            ArchiveValue av = new ArchiveValue();
            av.id = -1;
            av.dt = new DateTime();
            av.energy = -1;
            av.volume = -1;
            av.timeOn = -1;
            av.timeErr = -1;

            if (Meter.ReadArchiveLastVal(ref av))
            {
                string res = String.Format("ID: {5}\nДата: {4}\nЭнергия: {0}\nОбъем: {1}\nВремя работы: {2}\nВремя с ошибкой: {3}\n",
                    av.energy, av.volume, av.timeOn, av.timeErr, av.dt, av.id);
                addMessageToScreen(res);
            }
            else
            {
                addMessageToScreen("Метод драйвера ReadArchiveLastVal вернул false");
            }
        }

        private void infoBtn_Click(object sender, EventArgs e)
        {
            clearScreen();
            addMessageToScreen("*** Параметры по PT ***");
            addMessageToScreen("Общая информация об архиве: ");
            int cnt = 0, id = 0;
            if (Meter.ReadArchiveValCountId(ref cnt, ref id))
                addMessageToScreen(String.Format("Всего записей: {0}\nLID последней записи: {1}", cnt, id));
            else
                addMessageToScreen("Метод драйвера ReadArchiveValCountId вернул false");
        }


        private bool _portInitialized;
        /// <summary>
        /// Управляет доступностью интерфейса взаимодействия с драйвером
        /// </summary>
        public bool portInitialized
        {
            get
            {
                return _portInitialized;
            }
            set 
            {
                _portInitialized = value;
                if (value)
                {
                    groupBox2.Enabled = true;
                    groupBox1.Enabled = true;
                    groupBox4.Enabled = true;
                    apatorBtn.Enabled = true;
                }
                else
                {
                    groupBox2.Enabled = false;
                    groupBox1.Enabled = false;
                    apatorBtn.Enabled = false;
                    groupBox4.Enabled = false;                }
            }
        }

        private void readDailyBtn_Click(object sender, EventArgs e)
        {
            float val = -1f;
            Meter.ReadDailyValues(dateTimePicker1.Value, (ushort)numericUpDown2.Value, (ushort)numericUpDown3.Value, ref val);
            clearScreen();
            addMessageToScreen("*** Суточные ***");
            addMessageToScreen("Значение: ");
            addMessageToScreen(val.ToString());
        }

        private void readCurBtn_Click(object sender, EventArgs e)
        {
            float val = -1f;
            Meter.ReadCurrentValues((ushort)numericUpDown2.Value, (ushort)numericUpDown3.Value, ref val);
            clearScreen();
            addMessageToScreen("*** Текущие ***");
            addMessageToScreen("Значение: ");
            addMessageToScreen(val.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vp != null)
            {
                vp.closePort();
            }
        }


        private void addressCmbBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int val = Convert.ToInt32(addressCmbBox.Text, 16);
                var ind = addressCmbBox.Items.IndexOf(addressCmbBox.Text);
                if (ind == -1)
                {
                    addressCmbBox.SelectedIndex = addressDecCmb.SelectedIndex;
                    return;
                }
                addressCmbBox.SelectedIndex = ind;
                addressDecCmb.SelectedIndex = addressCmbBox.SelectedIndex;

                addressDecCmb.Select(addressDecCmb.Text.Length, 0);
                addressCmbBox.Select(addressDecCmb.Text.Length, 0);
            }
            catch (Exception ex)
            {

            }
        }


        private void addressDecCmb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int val = Convert.ToInt32(addressDecCmb.Text, 10);
                var ind = addressDecCmb.Items.IndexOf(addressDecCmb.Text);
                if (ind == -1)
                {
                    addressDecCmb.SelectedIndex = addressCmbBox.SelectedIndex;
                    return;
                }
                addressDecCmb.SelectedIndex = ind;
                addressCmbBox.SelectedIndex = addressDecCmb.SelectedIndex;
                addressDecCmb.Select(addressDecCmb.Text.Length, 0);
                addressCmbBox.Select(addressDecCmb.Text.Length, 0);
                
            }
            catch (Exception ex)
            {

            }
        }

        //Тест связи
        private void button1_Click(object sender, EventArgs e)
        {
            clearScreen();
            addMessageToScreen("*** Тест связм ***");

            if (!Meter.OpenLinkCanal())
            {
                addMessageToScreen("*** Не выполнен ***");
            }
            else
            {
                addMessageToScreen("*** ВЫПОЛНЕН ***");
            }
        }
        
        //Серийник
        private void button2_Click(object sender, EventArgs e)
        {
            clearScreen();
            addMessageToScreen("*** Серийный номер ***");

            string serial_str = "";
            if (!Meter.ReadSerialNumber(ref serial_str))
            {
                addMessageToScreen("*** Не считан ***");
            }
            else
            {
                addMessageToScreen(serial_str);
            }
        }

        //Ответ rsp_ud
        private void reqUd2_btn_Click(object sender, EventArgs e)
        {
            clearScreen();
            addMessageToScreen("*** Байты rsp_ud ***");
            string res_str = "Ошибка";

            List<byte> bytes = new List<byte>();
            if (Meter.SendREQ_UD2(out bytes))
            {
                addMessageToScreen("Команда: 5B");
                addMessageToScreen("Н/Конец: 4 + 2 = 6B");
                addMessageToScreen("C/A/CI: 3B");
                addMessageToScreen("Заголовок FDH: 12B");
                addMessageToScreen("");
                addMessageToScreen("Длина блока данных (Б): " + bytes.Count);
                addMessageToScreen("Длина сообщения (Б): " + (bytes.Count + 26));
                addMessageToScreen("");

                addMessageToScreen("Байты данных: ");
                res_str = BitConverter.ToString(bytes.ToArray(), 0).Replace("-", " ");
            }

            addMessageToScreen(res_str);
        }

        //Все
        private void button3_Click(object sender, EventArgs e)
        {
            clearScreen();

            addMessageToScreen("*** Параметры по MBUS ***");

            string resStr = "";

            if (!Meter.OpenLinkCanal())
            {
                addMessageToScreen("Провал теста связи");
                //return;
            }

            addMessageToScreen();
            Meter.GetAllValues(out resStr);
            addMessageToScreen(resStr);
            addMessageToScreen();

        }

    }
}
