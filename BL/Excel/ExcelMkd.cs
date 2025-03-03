using BE.PersData;
using BL.Services;
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
    public interface IExcelMkd
    {
        byte[] GetListFlats(int adressid);
    }

    public class ExcelMkd : IExcelMkd
    {
        private readonly IMkdInformationService _mkdInformationService;

        public ExcelMkd(IMkdInformationService mkdInformationService)
        {
            _mkdInformationService = mkdInformationService;
        }

        public byte[] GetListFlats(int adressid)
        {
            var listFlats = _mkdInformationService.GetListFlats(adressid);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Список помещений");

                worksheet.SetValue(1, 1, "Номер помещения");
                worksheet.SetValue(1, 2, "Тип помещения");
                worksheet.SetValue(1, 3, "Лицевой счет");
                worksheet.SetValue(1, 4, "Площадь");
                worksheet.SetValue(1, 5, "Количество проживающих");
                worksheet.SetValue(1, 6, "ФИО");
                worksheet.SetValue(1, 7, "ELS");
              
                int i = 2;
                foreach (var Items in listFlats)
                {
                    worksheet.SetValue(i, 1, Items.FlatNumber);
                    worksheet.SetValue(i, 2, Items.FlatType);
                    worksheet.SetValue(i, 3, Items.FullLic);
                    worksheet.SetValue(i, 4, Items.TotalSquare);
                    worksheet.SetValue(i, 5, Items.NumberOfPersons);
                    worksheet.SetValue(i, 6, Items.FIO);
                    worksheet.SetValue(i, 7, Items.Els);
                   
                    i++;
                }
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
    }
}
