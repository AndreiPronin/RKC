using BE.Counter;
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
        public static bool TypePuIsGvs(this TypePU? Value)
        {
            if(Value == null) return false;

            if(Value == TypePU.GVS1 || Value == TypePU.GVS2 || Value == TypePU.GVS3 || Value == TypePU.GVS4)
                return true;

            return false;
        }
    }
    public static class GetTypePuOfString
    {
        public static TypePU? GetTypePu(this string Value)
        {
            if (Value == "ГВС1")
                return TypePU.GVS1;
            if (Value == "ГВС2")
                return TypePU.GVS2;
            if (Value == "ГВС3")
                return TypePU.GVS3;
            if (Value == "ГВС4")
                return TypePU.GVS4;
            if (Value == "ОТП1")
                return TypePU.ITP1;
            if (Value == "ОТП2")
                return TypePU.ITP2;
            if (Value == "ОТП3")
                return TypePU.ITP3;
            if (Value == "ОТП4")
                return TypePU.ITP4;

            return null;
        }
    }
}
