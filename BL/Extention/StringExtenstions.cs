using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class StringExtenstions
    {
        public static string[] GetFioByString(this string value)
        {

            return value.Split(' ');
        }
    }
}
