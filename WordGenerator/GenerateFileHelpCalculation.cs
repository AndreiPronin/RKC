using Aspose.BarCode.Generation;
using BE.PersData;
using DB.DataBase;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace WordGenerator
{
    public static class GenerateFileHelpCalculation
    {
        public static PersDataDocumentLoad Generate(string LIC,DateTime date)
        {
            using (var db = new DbLIC())
            {
                var Lic = db.ALL_LICS_ARCHIVE.Where(x => x.F4ENUMELS == LIC && x.period.Value.Year == date.Year && x.period.Value.Month == date.Month).First();
                var WebAppDb = new ApplicationDbContext();
                var persData = WebAppDb.PersonalInformation.Where(x => x.full_lic == LIC).First();
                string path = AppDomain.CurrentDomain.BaseDirectory + $@"Template\";
                if (File.Exists(path + $@"Образец квитанции {LIC}.docx")) File.Delete(path + $@"Образец квитанции {LIC}.docx");
                File.Copy(Path.Combine(path, "Образец квитанции.docx"), Path.Combine(path, $@"Образец квитанции {LIC}.docx"), true);
                Application app = new Application();
                _Document doc = app.Documents.Open(path + $@"Образец квитанции {LIC}.docx");
                doc.Content.Find.Execute("{address}", false, true, false, false, false, true, 1, false, $@"{Lic.UL},дом {Lic.DOM},кв. {Lic.KW}", 2,
false, false, false, false);
                doc.Content.Find.Execute("{lic}", false, true, false, false, false, true, 1, false, Lic.F4ENUMELS, 2,
false, false, false, false);
                doc.Content.Find.Execute("{month}", false, true, false, false, false, true, 1, false, Lic.period.Value.ToString("MMMM,yyyy").Replace(","," ").ToUpper(), 2,
false, false, false, false);
                doc.Content.Find.Execute("{fio}", false, true, false, false, false, true, 1, false, Lic.FIO , 2,
false, false, false, false);
                doc.Content.Find.Execute("{sobs}", false, true, false, false, false, true, 1, false, Lic.SOBS, 2,
false, false, false, false);
                doc.Content.Find.Execute("{kl}", false, true, false, false, false, true, 1, false, Lic.KL, 2,
false, false, false, false);
                doc.Content.Find.Execute("{els}", false, true, false, false, false, true, 1, false, persData.els, 2,
false, false, false, false);
                doc.Content.Find.Execute("{s_nez}", false, true, false, false, false, true, 1, false, persData.S_NEZ, 2,
false, false, false, false);
                doc.Content.Find.Execute("{igku}", false, true, false, false, false, true, 1, false, persData.igku, 2,
false, false, false, false);
                doc.Content.Find.Execute("{S_GIL}", false, true, false, false, false, true, 1, false, persData.S_GIL, 2,
false, false, false, false);
                doc.Content.Find.Execute("{dpukanach}", false, true, false, false, false, true, 1, false, persData.S_NEZ, 2,
false, false, false, false);
                doc.Content.Find.Execute("{S_OI}", false, true, false, false, false, true, 1, false, persData.S_OI, 2,
false, false, false, false);
                doc.Content.Find.Execute("{s_notp}", false, true, false, false, false, true, 1, false, persData.S_NOTP, 2,
false, false, false, false);
                //BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code39Standard, "1234567890");
                //Stream ms = new MemoryStream();
                //generator.Save(ms, BarCodeImageFormat.Bmp);
                //generator.Save(path + "test.png", BarCodeImageFormat.Png);
                BarcodeWriter generator = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                generator.Options = new ZXing.Common.EncodingOptions
                {
                    Width = 100,
                    Height = 100
                };
                generator.Write("111111111111111111111111111").Save(path + $@"{LIC}.png");
                generator = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                generator.Options = new ZXing.Common.EncodingOptions
                {
                    Width = 50,
                    Height = 50
                };
                generator.Write("111111111111111111111111111").Save(path + $@"{LIC}_QR.png");
                try
                {
                    Microsoft.Office.Interop.Word.Range shtrix = doc.Range(0, 0);
                    Microsoft.Office.Interop.Word.Range qr = doc.Range(0, 0);
                    shtrix.Find.Execute("{shtrix}");
                    qr.Find.Execute("{qr}");
                    doc.Content.Find.Execute("{shtrix}", false, true, false, false, false, true, 1, false, "", 2,
false, false, false, false);
                    doc.Content.Find.Execute("{qr}", false, true, false, false, false, true, 1, false, "", 2,
false, false, false, false);
                    Range rangeImgShtrix = doc.Range(shtrix.Start, shtrix.End);
                    Range rangeImgQr = doc.Range(qr.Start, qr.End);
                    var ImgShtrix = doc.InlineShapes.AddPicture(path + $@"{LIC}.png", false, true, rangeImgShtrix).ConvertToShape();
                    ImgShtrix.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapBehind;
                    var ImgQr = doc.InlineShapes.AddPicture(path + $@"{LIC}_QR.png", false, true, rangeImgQr).ConvertToShape();
                    ImgQr.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapBehind;

                }
                catch { }
                doc.Save();
                doc.ExportAsFixedFormat(path + $@"Образец квитанции {LIC}.pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                   Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                   false);
                app.Quit();
                if (File.Exists(path + $@"Образец квитанции {LIC}.docx")) File.Delete(path + $@"Образец квитанции {LIC}.docx");
                if (File.Exists(path + $@"{LIC}.png")) File.Delete(path + $@"{LIC}.png");
                if (File.Exists(path + $@"{LIC}_QR.png")) File.Delete(path + $@"{LIC}_QR.png");

                return new PersDataDocumentLoad() { FileBytes = File.ReadAllBytes(path + $@"Образец квитанции {LIC}.pdf"), FileName = $@"Квитанция {LIC}.pdf"};
            }

        }
    }
}
