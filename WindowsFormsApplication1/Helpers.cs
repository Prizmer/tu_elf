using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication1
{
    public class Helpers
    {
        /// <summary>
        /// Запись в ЛОГ-файл
        /// </summary>
        /// <param name="str"></param>
        public void WriteToLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;

                try
                {
                    sw = new StreamWriter("elflog.log", true);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + str);
                    sw.Close();
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
                }
            }
        }
    }
}
