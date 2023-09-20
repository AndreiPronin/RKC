using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelsReader.Extenstions
{
    public static class Functions
    {
        public static string GetIp(this string Value)
        {
            if (Value != null && !Value.Contains("Погашение долга"))
            {
                var result = "";
                var r1 = Value.Split(' ').TryGetValue(2);
                var r2 = Value.Split(' ').TryGetValue(3);
                var r3 = Value.Split(' ').TryGetValue(4);
                result = r1 + r2;
                return result;
            }
            else return "";
        }
        public static string GetCourtWork(this string Value)
        {
            if (!string.IsNullOrEmpty(Value)){
                var result = "";
                var index = Value.IndexOf("Судебный приказ ");
                if (index == -1)
                    return "";
                var probell = 0;
                for (int i = index; i <= Value.Length - 1; i++)
                {
                    if (Value[i] == ' ')
                        probell ++;

                    if (probell == 5)
                        break;
                    result += Value[i];
                }
                return result;
            }
            return "";
        }
        public static string GetFio(this string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var result = "";
                var index = Value.IndexOf("долг с") + 6;
                if (index == 5)
                {
                    index = Value.IndexOf(" долга: ") + 8;
                }
                var probell = 0;
                for (int i = index; i <= Value.Length - 1; i++)
                {
                    if (Value[i] == ' ')
                        probell++;

                    if (probell == 4)
                        break;
                    result += Value[i];
                }
                return result;
            }
            return "";
        }
        public static string TryGetValue(this string[] Value, int postion)
        {
            try
            {
                var result = Value[postion];
                return result;
            }catch {
                return "";
            }
        }
    }
}
