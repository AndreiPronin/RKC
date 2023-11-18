using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Loggers
{
    public class ShedulerLogger
    {
        public static void WhriteToFile(string s)
        {
            if (string.IsNullOrEmpty(s))
                return;
            var FilePath = AppDomain.CurrentDomain.BaseDirectory + $@"Logs\{DateTime.Now.ToString("yyyy-MM-dd")} Sheduler.log";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"Logs"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $@"Logs");
            }
            if (File.Exists(FilePath))
            {
                var OldText = File.ReadAllText(FilePath);
                s += Environment.NewLine;
                s += OldText;
                File.WriteAllText(FilePath, s);
            }
            else
            {
                File.Create(FilePath).Close();

                File.WriteAllText(FilePath, s + Environment.NewLine);
            }
        }
    }
}
