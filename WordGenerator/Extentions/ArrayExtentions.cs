using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGenerator.Extentions
{
    public static class ArrayExtentions
    {
        public static string TryGetValue(this string[] s, int index)
        {
            try
            {
                return s[index];
            }
            catch
            {
                return "";
            }
        }
    }
}
