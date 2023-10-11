using ClosedXML.Excel;
using DB.Model;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace BL.Excel
{
    public static class ExcelReport
    {
        public static byte[] Generate(List<List<object>> lists)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Отчет");
                var lasObj = lists.FirstOrDefault();
                int i = 1;
                int z = 1;
                foreach(var Items in lists)
                {
                    z = 1;
                    foreach (var Item in Items)
                    {
                        worksheet.SetValue(i, z, Item);
                        z++;
                    }
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
        public static int Generate(List<List<object>> lists, IXLWorksheet worksheet)
        {
            var lasObj = lists.FirstOrDefault();
            int i = 2;
            int z = 1;
            foreach (var Items in lists)
            {
                z = 1;
                foreach (var Item in Items)
                {
                    worksheet.SetValue(i, z, Item);
                    z++;
                }
                i++;
            }
            return i;
        }
    }
}
