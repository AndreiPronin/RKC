using BE.PersData;
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
        public static byte[] Generate(List<HelpCalculationsModel> helpCalculations)
        {
            helpCalculations = helpCalculations.OrderBy(x=>x.Period).ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Справка расчёт");
                worksheet.MergeAndValue(3,5,3,10, $"Справка по лицевому счету № {helpCalculations.First().LIC}" );
                worksheet.Cell(3,5).Style.Font.Bold = true;
                worksheet.Column(1).Width = 25.0;
                //worksheet.Column(11).Width = 17.0;
                //worksheet.Column(12).Width = 17.0;
                //worksheet.Column(13).Width = 17.0;
                worksheet.Column(14).Width = 17.0;
                //worksheet.Column(15).Width = 17.0;
                //worksheet.Column(16).Width = 17.0;
                //worksheet.Column(17).Width = 17.0;
                worksheet.Column(18).Width = 17.0;
                worksheet.Column(22).Width = 17.0;
                worksheet.Column(23).Width = 17.0;
                worksheet.Column(23).Style.Alignment.WrapText = true;
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
                worksheet.MergeAndValue(9, 11, 9, 23, $"Детализация по услугам");
                worksheet.MergeAndValue(10, 1, 12, 1, $"период");
                worksheet.MergeAndValue(10, 2, 10, 3, $"Вх. сальдо");
                worksheet.MergeAndValue(11, 2,12,2, "ЖКУ");
                worksheet.MergeAndValue(11, 3, 12, 3, "пени");
                worksheet.MergeAndValue(10, 4, 10, 5, $"Оплачено");
                worksheet.MergeAndValue(11, 4, 12, 4, "ЖКУ");
                worksheet.MergeAndValue(11, 5, 12, 5, "пени");
                worksheet.MergeAndValue(10, 6, 10, 7, $"Начислено");
                worksheet.MergeAndValue(11, 6, 12, 6, "ЖКУ");
                worksheet.MergeAndValue(11, 7, 12, 7, "пени");
                worksheet.MergeAndValue(10, 8, 10, 9, $"Исх. Сальдо");
                worksheet.MergeAndValue(11, 8, 12, 8, "ЖКУ");
                worksheet.MergeAndValue(11, 9, 12, 9, "пени");
                worksheet.MergeAndValue(10, 10, 12, 10, $"К оплате");
                worksheet.MergeAndValue(10, 8, 10, 9, $"Исх. Сальдо");
                worksheet.MergeAndValue(10, 11, 10, 14, $"Отопление");
                worksheet.MergeAndValue(11, 11, 11, 13, $"начислено");
                worksheet.SetValue(12, 11, "Гкал");
                worksheet.SetValue(12, 12, "тариф");
                worksheet.SetValue(12, 13, "руб.");
                worksheet.SetValue(12, 14, "руб.");
                worksheet.SetValue(11, 14, "перерасчет");
                worksheet.MergeAndValue(10, 15, 10, 18, $"ГВС компонент ТЭ");
                worksheet.MergeAndValue(11, 15, 11, 17, $"начислено");
                worksheet.SetValue(12, 15, "Гкал");
                worksheet.SetValue(12, 16, "тариф");
                worksheet.SetValue(12, 17, "руб.");
                worksheet.SetValue(12, 18, "руб.");
                worksheet.SetValue(11, 18, "перерасчет");
                worksheet.MergeAndValue(10, 19, 10, 22, $"ГВС компонент ХВ");
                worksheet.MergeAndValue(11, 19, 11, 21, $"начислено");
                worksheet.SetValue(12, 19, "м3");
                worksheet.SetValue(12, 20, "тариф");
                worksheet.SetValue(12, 21, "руб.");
                worksheet.SetValue(12, 22, "руб.");
                worksheet.SetValue(11, 22, "перерасчет");
                worksheet.MergeAndValue(10, 23, 12, 23, $"Корректир. сальдо ЖКУ");
                int i = 13;
                foreach (var Items in helpCalculations)
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

                    worksheet.SetValue(i, 11, Items.HeatingСalculationGcal);
                    worksheet.SetValue(i, 12, Items.HeatingRecalculationRate);
                    worksheet.SetValue(i, 13, Items.HeatingСalculation);
                    worksheet.SetValue(i, 14, Items.HeatingRecalculation);
                    worksheet.SetValue(i, 15, Items.GvsHeatingСalculationGcal);
                    worksheet.SetValue(i, 16, Items.GvsHeatingRecalculationRate);
                    worksheet.SetValue(i, 17, Items.GvsHeatingСalculation);
                    worksheet.SetValue(i, 18, Items.GvsHeatingRecalculation);
                    worksheet.SetValue(i, 19, Items.HvHeatingСalculationGcal);
                    worksheet.SetValue(i, 20, Items.HvHeatingRecalculationRate);
                    worksheet.SetValue(i, 21, Items.HvHeatingСalculation);
                    worksheet.SetValue(i, 22, Items.HvHeatingRecalculation);

                    worksheet.SetValue(i, 23, Items.SN15);
                    i++;
                }
                worksheet.SetValue(i, 1, "Итого:");
                worksheet.Cell("D" + i).FormulaA1 = $"sum(D13:D{i - 1})";
                worksheet.Cell("E" + i).FormulaA1 = $"sum(E13:E{i - 1})";
                worksheet.Cell("F" + i).FormulaA1 = $"sum(F13:F{i - 1})";
                worksheet.Cell("G" + i).FormulaA1 = $"sum(G13:G{i - 1})";

                worksheet.Cell("M" + i).FormulaA1 = $"sum(M13:M{i - 1})";
                worksheet.Cell("N" + i).FormulaA1 = $"sum(N13:N{i - 1})";
                worksheet.Cell("Q" + i).FormulaA1 = $"sum(Q13:Q{i - 1})";
                worksheet.Cell("R" + i).FormulaA1 = $"sum(R13:R{i - 1})";
                worksheet.Cell("U" + i).FormulaA1 = $"sum(U13:U{i - 1})";
                worksheet.Cell("V" + i).FormulaA1 = $"sum(V13:V{i - 1})";
                worksheet.Cell("W" + i).FormulaA1 = $"sum(W13:W{i - 1})";
                worksheet.Range(9, 1, i, 23).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(9, 1, i, 23).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
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
