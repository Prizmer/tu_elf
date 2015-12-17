using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using Prizmer.Ports;


namespace Prizmer.Meters
{

    /// <summary>
    /// Описывает типовое архивное значение
    /// </summary>
    public struct ArchiveValue
    {
        public int id;
        public DateTime dt;
        public float energy;
        public float volume;
        public int timeOn;
        public int timeErr;
    };

    public class CMeter
    {

        public VirtualPort m_vport = null;
        public uint m_address = 0;

        /// <summary>
        /// Запись в ЛОГ-файл
        /// </summary>
        /// <param name="str"></param>
        public void WriteToLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;
                FileStream fs = null;
                try
                {
                    fs = new FileStream(@"elflog.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    if (m_vport == null) sw.WriteLine(DateTime.Now.ToString() + ": Unknown port: adress: " + m_address + ": " + str);
                    else sw.WriteLine(DateTime.Now.ToString() + ": " + m_vport.GetName() + ": adress: " + m_address + ": " + str);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
    }
}
