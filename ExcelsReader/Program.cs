using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using ExcelsReader.Extenstions;
using ExcelsReader.Models;
using ExcelsReader.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelsReader
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            List<Court> courts = new List<Court>();
            FilesServies filesServies = new FilesServies();
            var result = filesServies.GetAllExcelFile(Environment.CurrentDirectory.Replace("\\bin\\Debug","")+"\\FilesFolder");
            foreach (var file in result)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file);
                    var Rows = workbook.Worksheet(1).RowsUsed();
                    foreach (var row in Rows)
                    {
                        if (row.RowNumber()> 1)
                        {
                            var court = new Court();
                            court.Lic = row.Cell(1).Value.ToString();
                            court.Ip = row.Cell(2).Value.ToString().GetIp();
                            court.CourtWork = row.Cell(2).Value.ToString().GetCourtWork();
                            court.Fio = row.Cell(2).Value.ToString().GetFio();
                            court.FileName = file.Replace("D:\\Programing\\RKC\\ExcelsReader\\FilesFolder\\","");
                            courts.Add(court);
                        }
                    }
                }
            }
            using (var wbook = new XLWorkbook())
            {
                int i = 2;

                var ws = wbook.Worksheets.Add("Sheet1");
                foreach(var Item in courts)
                {
                    ws.Cell(i, 1).Value = Item.Lic;
                    ws.Cell(i, 2).Value = Item.Ip;
                    ws.Cell(i, 3).Value = Item.CourtWork;
                    ws.Cell(i, 4).Value = Item.Fio;
                    ws.Cell(i, 5).Value = Item.FileName;
                    i++;
                }
                wbook.SaveAs("result.xlsx");
            }
        }
    }
}
