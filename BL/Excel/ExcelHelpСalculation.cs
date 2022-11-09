using ClosedXML.Excel;
using DB.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public static class ExcelHelpСalculation
    {
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, string value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void SetValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst, decimal? value)
        {
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
        }
        public static void MergeAndValue(this IXLWorksheet xLWorksheet, int rowFirst, int columnFirst,  int rowLast, int columnLast, string value)
        {
            xLWorksheet.Range(rowFirst, columnFirst, rowLast, columnLast).Merge();
            xLWorksheet.Cell(rowFirst, columnFirst).Value = value;
            xLWorksheet.Cell(rowFirst, columnFirst).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
        public static byte[] Generate(List<HelpСalculations> helpCalculations)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Справка расчёт");
                worksheet.MergeAndValue(3,5,3,10, $"Справка по лицевому счету № {helpCalculations.First().LIC}" );
                worksheet.Cell(3,5).Style.Font.Bold = true;
                worksheet.Column(1).Width = 25.0;
                worksheet.Column(11).Width = 17.0;
                worksheet.Column(12).Width = 17.0;
                worksheet.Column(13).Width = 17.0;
                worksheet.Column(14).Width = 17.0;
                worksheet.Column(15).Width = 17.0;
                worksheet.Column(16).Width = 17.0;
                worksheet.Column(17).Width = 17.0;
                worksheet.Column(17).Style.Alignment.WrapText = true;
                worksheet.SetValue(5, 1, "Адрес:");
                worksheet.SetValue(5, 2, $"{helpCalculations.First().UL.ToLower().toUpperFirst().Trim()} дом {helpCalculations.First().DOM.Trim()} {helpCalculations.First().KW.Trim()}");
                worksheet.SetValue(6, 1, "ФИО:");
                try
                {
                    worksheet.SetValue(6, 2, $"{helpCalculations.First().FIO.Split()[0].ToLower().toUpperFirst()} {helpCalculations.First().FIO.Split()[1].ToLower().toUpperFirst()} {helpCalculations.First().FIO.Split()[2].ToLower().toUpperFirst()}");
                }
                catch { }
                worksheet.SetValue(7, 1, "Площадь:");
                worksheet.SetValue(7, 2, $"{helpCalculations.First().Square}");
                worksheet.SetValue(8, 1, "Количество человек:");
                worksheet.SetValue(8, 2, $"{helpCalculations.First().NumberPerson}");
                worksheet.MergeAndValue(9, 1, 9, 10, $"Состояние расчётов");
                worksheet.MergeAndValue(9, 11, 9, 17, $"Детализация по услугам");
                worksheet.MergeAndValue(10, 1, 11, 1, $"период");
                worksheet.MergeAndValue(10, 2, 10, 3, $"Вх. сальдо");
                worksheet.SetValue(11, 2, "ЖКУ");
                worksheet.SetValue(11, 3, "пени");
                worksheet.MergeAndValue(10, 4, 10, 5, $"Оплачено");
                worksheet.SetValue(11, 4, "ЖКУ");
                worksheet.SetValue(11, 5, "пени");
                worksheet.MergeAndValue(10, 6, 10, 7, $"Начислено");
                worksheet.SetValue(11, 6, "ЖКУ");
                worksheet.SetValue(11, 7, "пени");
                worksheet.MergeAndValue(10, 8, 10, 9, $"Исх. Сальдо");
                worksheet.SetValue(11, 8, "ЖКУ");
                worksheet.SetValue(11, 9, "пени");
                worksheet.MergeAndValue(10, 10, 11, 10, $"К оплате");
                worksheet.MergeAndValue(10, 8, 10, 9, $"Исх. Сальдо");
                worksheet.SetValue(11, 8, "ЖКУ");
                worksheet.SetValue(11, 9, "пени");
                worksheet.MergeAndValue(10, 11, 10, 12, $"Отопление");
                worksheet.SetValue(11,11, "начислено");
                worksheet.SetValue(11, 12, "перерасчет");
                worksheet.MergeAndValue(10, 13, 10, 14, $"ГВС компонент ТЭ");
                worksheet.SetValue(11, 13, "начислено");
                worksheet.SetValue(11, 14, "перерасчет");
                worksheet.MergeAndValue(10, 15, 10, 16, $"ГВС компонент ХВ");
                worksheet.SetValue(11, 15, "начислено");
                worksheet.SetValue(11, 16, "перерасчет");
                worksheet.MergeAndValue(10, 17, 11, 17, $"Корректир. сальдо ЖКУ");
                int i = 12;
                foreach(var Items in helpCalculations)
                {
                    worksheet.SetValue(i, 1, Items.Period.Value.ToString("MMM.yyyy"));
                    worksheet.SetValue(i, 2, Items.DK);
                    worksheet.SetValue(i, 3, Items.Peny_dk);
                    worksheet.SetValue(i, 4, Items.Sp);
                    worksheet.SetValue(i, 5, Items.Peny);
                    worksheet.SetValue(i, 6, Items.SN);
                    worksheet.SetValue(i, 7, Items.PenySNpenySR);
                    worksheet.SetValue(i, 8, Items.Tdk);
                    worksheet.SetValue(i, 9, Items.Peny_tdk);
                    worksheet.SetValue(i, 10, Items.TdkPeny_tdk);
                    worksheet.SetValue(i, 11, Items.HeatingСalculation);
                    worksheet.SetValue(i, 12, Items.HeatingRecalculation);
                    worksheet.SetValue(i, 13, Items.GvsHeatingСalculation);
                    worksheet.SetValue(i, 14, Items.GvsHeatingRecalculation);
                    worksheet.SetValue(i, 16, Items.HvHeatingСalculation);
                    worksheet.SetValue(i, 16, Items.HvHeatingRecalculation);
                    worksheet.SetValue(i, 17, Items.SN15);
                    i++;
                }
                worksheet.SetValue(i, 1, "Итого:");
                worksheet.Cell("D"+ i).FormulaA1 = $"sum(D12:D{i-1})";
                worksheet.Cell("E" + i).FormulaA1 = $"sum(E12:E{i - 1})";
                worksheet.Cell("F" + i).FormulaA1 = $"sum(F12:F{i - 1})";
                worksheet.Cell("G" + i).FormulaA1 = $"sum(G12:G{i - 1})";
                worksheet.Cell("K" + i).FormulaA1 = $"sum(K12:K{i - 1})";
                worksheet.Cell("L" + i).FormulaA1 = $"sum(L12:L{i - 1})";
                worksheet.Cell("M" + i).FormulaA1 = $"sum(M12:M{i - 1})";
                worksheet.Cell("N" + i).FormulaA1 = $"sum(N12:N{i - 1})";
                worksheet.Cell("O" + i).FormulaA1 = $"sum(O12:O{i - 1})";
                worksheet.Cell("P" + i).FormulaA1 = $"sum(P12:P{i - 1})";
                worksheet.Cell("Q" + i).FormulaA1 = $"sum(Q12:Q{i - 1})";
                worksheet.Range(9, 1, i, 17).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(9, 1, i, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                i++;
                worksheet.MergeAndValue(i+3, 1, i+3, 7, $"Исполнитель__________________________________________________");
                worksheet.Cell(i + 3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(i + 3, 1).Style.Font.Bold = true;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var writer = new StreamWriter(stream);
                    writer.Flush();
                    stream.Position = 0;
                    return stream.ToArray();
                }
            }
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
