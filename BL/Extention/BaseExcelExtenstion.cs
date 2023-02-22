using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public static class BaseExcelExtenstion
    {
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, string value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, object value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).SetDataType(XLDataType.Text);
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, decimal? value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, double? value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, DateTime? value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void MergeAndValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, int rowLast, int columnLast, string value)
        {
            xLWorksheet.Range(rowFirst, columnFirst, rowLast, columnLast).Merge();
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
            xLWorksheet.Cell(rowFirst, columnFirst).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
        public static string toUpperFirst(this string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}
