using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Drivers.ElfApatorDriver;

namespace WindowsFormsApplication1
{
    public partial class HelpUtilsForm : Form
    {
        public HelpUtilsForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string inp_hex_str = richTextBox4.Text.Replace(" ", String.Empty).Replace("-", String.Empty);
            try
            {
                if (inp_hex_str.Length % 2 != 0 || inp_hex_str == "") return;

                byte[] tmpArr = new byte[inp_hex_str.Length / 2];


                for (int i = 0; i < inp_hex_str.Length; i += 2)
                    tmpArr[i / 2] = Convert.ToByte(inp_hex_str.Substring(i, 2), 16);

                byte[] encryptedBytes = new byte[tmpArr.Length];
                elfDriver.EncryptByteArr(tmpArr, ref encryptedBytes);
                string encryptedStr = BitConverter.ToString(encryptedBytes).Replace("-", " ").ToLower();

                richTextBox5.Text = encryptedStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
          string inp_hex_str = richTextBox4.Text.Replace(" ", String.Empty).Replace("-", String.Empty);
            try
            {
                if (inp_hex_str.Length % 2 != 0 || inp_hex_str == "") return;

                byte[] tmpArr = new byte[inp_hex_str.Length / 2];


                for (int i = 0; i < inp_hex_str.Length; i += 2)
                    tmpArr[i / 2] = Convert.ToByte(inp_hex_str.Substring(i, 2), 16);

                byte[] decryptedBytes = new byte[tmpArr.Length];
                elfDriver.DecryptByteArr(tmpArr, ref decryptedBytes);
                string decryptedStr = BitConverter.ToString(decryptedBytes).Replace("-", " ").ToLower();

                richTextBox5.Text = decryptedStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string inp_hex_str = richTextBox3.Text.Replace(" ", String.Empty).Replace("-", String.Empty);
            try
            {
                if (inp_hex_str.Length % 2 != 0 || inp_hex_str == "") return;

                byte[] tmpArr = new byte[inp_hex_str.Length / 2];


                for (int i = 0; i < inp_hex_str.Length; i += 2)
                    tmpArr[i / 2] = Convert.ToByte(inp_hex_str.Substring(i, 2), 16);

                byte[] crcArr = new byte[1];
                crcArr[0] = elfDriver.CRC8(tmpArr, tmpArr.Length);
                string crcStr = BitConverter.ToString(crcArr).Replace("-", " ").ToLower();
                textBox3.Text = crcStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ElfApatorDriver elfDriver = new ElfApatorDriver();
        private void HelpUtilsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
