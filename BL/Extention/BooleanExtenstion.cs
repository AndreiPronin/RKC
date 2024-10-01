using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class BooleanExtenstion
    {
        public static string BoolToString(this bool? val)
        {
            switch (val)
            {
                case true: return "Да";
                case false: return "Нет";
                default: return "Нет";
            }
        }
    }
}
