﻿using ClosedXML.Excel;
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
        public static void SetValue(this IXLCell xlCell, string value)
        {
            xlCell.Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, object value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).SetDataType(XLDataType.Text);
            xLWorksheet.Cell(rowFirst, columnFirst).DataType = XLDataType.Text;
            if(double.TryParse(value.ToString(), out double resultDouble))
            {
                xLWorksheet.Cell(rowFirst, columnFirst).Value = resultDouble;
                return;
            }
            if (int.TryParse(value.ToString(),out int resultInt))
            {
                xLWorksheet.Cell(rowFirst, columnFirst).Value = resultInt;
                return;
            }
          

            var IsDate = DateTime.TryParse(value.ToString(), out DateTime outDateTime);
            if (IsDate == true)
            {
                if (value.ToString().Split('.').Count() < 3)
                {
                    value = $"'{value}";
                }
                xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
            }
            var res = Convert.ToString(value);
            xLWorksheet.Cell(rowFirst, columnFirst).Value = res;

        }
        public static IXLCell SetDataType(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, XLDataType dataType )
        {
            xLWorksheet.Cell(rowFirst, columnFirst).SetDataType(dataType);
            xLWorksheet.Cell(rowFirst, columnFirst).DataType = dataType;
            return xLWorksheet.Cell(rowFirst, columnFirst);
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, decimal? value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, int? value)
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
        public static string TryGetLic(this object row)
        {
            try
            {
                var result = row.ToString();
                if (!string.IsNullOrEmpty(result))
                    return result;
                throw new Exception("Пустой лицевой счет");
            }catch (Exception e)
            {
                throw e; 
            }
        }
        public static int TryGetCardNumber(this object row)
        {
            try
            {
                var result = row.ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    return Convert.ToInt32(result.Replace("П-", ""));
                }
                throw new Exception("Пустой номер карточки");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
