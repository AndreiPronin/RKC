using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public static class Excels
    {
        public static bool RowEmpty(params object[] rows)
        {
            return rows.Length == 0;
        }
    }
}
