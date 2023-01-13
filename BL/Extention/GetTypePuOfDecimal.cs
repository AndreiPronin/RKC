using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class GetTypePuOfDecimal
    {
        public static string GetTypePu(this decimal? Decimal, string NameDecimal)
        {
            if (NameDecimal == "FKUBSXVS")
                return "ГВС1";
            if (NameDecimal == "FKUBSXV_2")
                return "ГВС2";
            if (NameDecimal == "FKUBSXV_3")
                return "ГВС3";
            if (NameDecimal == "FKUBSXV_4")
                return "ГВС4";
            if (NameDecimal == "FKUBSOT_1")
                return "ОТП1";
            if (NameDecimal == "FKUBSOT_2")
                return "ОТП2";
            if (NameDecimal == "FKUBSOT_3")
                return "ОТП3";
            if (NameDecimal == "FKUBSOT_4")
                return "ОТП4";

            return string.Empty;
        }
    }
}
