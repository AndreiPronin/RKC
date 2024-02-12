using AppCache;
using BE.PersData;
using DB.DataBase;
using DB.Model;
using DB.Query;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenerator.Extentions;
using WordGenerator.interfaces;
using ZXing;

namespace WordGenerator
{
    public class ReceiptNewDPU : IPdfGenerate
    {
        public PersDataDocumentLoad Generate(string LIC, DateTime date)
        {
            ICacheApp cacheApp = new CacheApp();
            if (cacheApp.GetValueProgress(LIC) != null)
                cacheApp.Delete(LIC);
            cacheApp.AddProgress(LIC, $"Получаю данные из БД за {date}");
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Lic = db.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).FirstOrDefault(x => x.NewFullLic == LIC && x.Period.Year == date.Year && x.Period.Month == date.Month);
                    if (Lic == null)
                    {
                        cacheApp.AddProgress(LIC, "Ничего не найдено за выбранный период");
                        throw new Exception("Ничего не найдено за выбранный период");
                    }
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"\Template\KvitDPU\{date:MMMM-yyyy}"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $@"\Template\KvitDPU\{date:MMMM-yyyy}");
                    string path = AppDomain.CurrentDomain.BaseDirectory + $@"Template\KvitDPU\{date:MMMM-yyyy}\";
                    if (File.Exists(path + $@"Образец квитанции DPUNew {LIC} {date.Month}.docx")) File.Delete(path + $@"Образец квитанции DPUNew {LIC} {date.Month}.docx");

                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + $@"Template\Образец квитанции DPUNew.docx",
                        AppDomain.CurrentDomain.BaseDirectory + $@"Template\KvitDPU\{date:MMMM-yyyy}\Образец квитанции DPUNew {LIC} {date.Month}.docx");
                    cacheApp.Update(LIC, $"Начинаю формировать квитанцию за {date.ToString("dd-MMMM-yyyy")}");
                    Application app = new Application();
                    var TempFile = $@"Образец квитанции DPUNew {LIC} {date.Month}.docx";
                    _Document doc = app.Documents.Open(path + TempFile);
                    try
                    {
                        doc.Content.Find.Execute("{street}", false, true, false, false, false, true, 1, false, $@"{Lic.Street.Trim()}", 2,
    false, false, false, false);
                        doc.Content.Find.Execute("{Home}", false, true, false, false, false, true, 1, false, $@"{Lic.Home.Trim()}", 2,
    false, false, false, false);
                        doc.Content.Find.Execute("{Flat}", false, true, false, false, false, true, 1, false, $@"{Lic.Flat.Trim()}", 2,
    false, false, false, false);
                        doc.Content.Find.Execute("{FullName}", false, true, false, false, false, true, 1, false, $@"{Lic.FullName.Trim()}", 2,
   false, false, false, false);
                        doc.Content.Find.Execute("{NewFullLic}", false, true, false, false, false, true, 1, false, $@"{Lic.NewFullLic.Trim()}", 2,
   false, false, false, false);
                        doc.Content.Find.Execute("{period}", false, true, false, false, false, true, 1, false, Lic.Period.ToString("MMMM,yyyy").Replace(",", " ").ToUpper(), 2,
        false, false, false, false);

                        doc.Content.Find.Execute("{TotalAreaMKD}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalAreaMKD}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalAreaMKDResidentialPremises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalAreaMKDResidentialPremises}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalAreaMKDNonResidentialPremises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalAreaMKDNonResidentialPremises}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalCostOdpu}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalCostOdpu}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalCostOdpuResidentialPremises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalCostOdpuResidentialPremises}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalCostOdpuNonResidentialPremises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalCostOdpuNonResidentialPremises}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{SaldoBeginningPeriod}", false, true, false, false, false, true, 1, false, $@"{Lic.SaldoBeginningPeriod}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{Paid}", false, true, false, false, false, true, 1, false, $@"{Lic.Paid}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{TotalAccrued}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalAccrued}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{ToPay}", false, true, false, false, false, true, 1, false, $@"{Lic.ToPay}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{Sobs}", false, true, false, false, false, true, 1, false, $@"{Lic.Sobs}", 2,
  false, false, false, false);
                        doc.Content.Find.Execute("{OneTimePayment}", false, true, false, false, false, true, 1, false, $@"{Lic.OneTimePayment}", 2,
 false, false, false, false);
                        doc.Content.Find.Execute("{TotalCostOdpuResidentialPremises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalCostOdpuResidentialPremises}", 2,
 false, false, false, false);
                        doc.Content.Find.Execute("{TotalAccrued}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalAccrued}", 2,
 false, false, false, false);
                        doc.Content.Find.Execute("{ShareInCommonOwnership}", false, true, false, false, false, true, 1, false, $@"{Lic.ShareInCommonOwnership}", 2,
false, false, false, false);
                        doc.Content.Find.Execute("{Premises}", false, true, false, false, false, true, 1, false, $@"{Lic.TotalCostOdpuResidentialPremises}", 2,
false, false, false, false);
                        var SaldoSumm = Lic.SaldoEndPeriodDebt + Lic.SaldoEndPeriodPercentage;
                        SaldoSumm = SaldoSumm.HasValue ? Math.Round(SaldoSumm.Value,2) : SaldoSumm;
                        doc.Content.Find.Execute("{SaldoEndPeriodDebt}+{SaldoEndPeriodPercentage}", false, true, false, false, false, true, 1, false, $@"{SaldoSumm}", 2,
 false, false, false, false);
                        cacheApp.Update(LIC, $"Сформировал квитацнию за {date}");
                        BarcodeWriter generator = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                        generator.Options = new ZXing.Common.EncodingOptions
                        {
                            Width = 250,
                            Height = 250,
                            Margin = 2
                        };
                        var FIO = Lic.FullName.Trim().Split(' ');
                        var sum = Convert.ToDouble(Lic.ToPay.ToString().Trim().Replace(".", ",")) * 100;
                        var STR = $@"ST00011|Name=Пензенский филиал ПАО 'Т Плюс'|PersonalAcc=40702810748000001123|
BankName=Пензенское отделение № 8624 ПАО 'Сбербанк России' г. Пенза|BIC=045655635|CorrespAcc=30101810000000000635|PayeeINN=6315376946|
Category=7|PersAcc={LIC}|LastName={FIO.TryGetValue(0)}|FitstName={FIO.TryGetValue(1)}|MiddleName={FIO.TryGetValue(2)}
|PayerAddress={Lic.Street.Trim()}, дом {Lic.Home.Trim()}, кв. {Lic.Flat.Trim()}|Sum={sum}";
                        generator.Write(STR).Save(path + $@"{LIC}_QR.png");
                        try
                        {
                            Microsoft.Office.Interop.Word.Range qr = doc.Range(0, 0);
                            qr.Find.Execute("{qr}");
                            doc.Content.Find.Execute("{qr}", false, true, false, false, false, true, 1, false, "", 2,
        false, false, false, false);
                            Range rangeImgQr = doc.Range(qr.Start, qr.End);
                            var ImgQr = doc.InlineShapes.AddPicture(path + $@"{LIC}_QR.png", false, true, rangeImgQr).ConvertToShape();
                            ImgQr.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapFront;

                        }
                        catch (Exception ex) { }

                        BarcodeWriter generator1 = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                        ZXing.QrCode.QrCodeEncodingOptions opt = new ZXing.QrCode.QrCodeEncodingOptions
                        {
                            CharacterSet = "windows-1251",
                            Width = 500,
                            Height = 500,
                            Margin = 2
                        };
                        generator1.Options = opt;
                       
                        var FIO1 = Lic.FullName.Trim().Split(' ');
                        var sum1 = Convert.ToDouble(Lic.OneTimePayment.ToString().Trim().Replace(".", ",")) * 100;
                        var STR1 = $@"ST00011|Name=Пензенский филиал ПАО 'Т Плюс'|PersonalAcc=40702810748000001123|
BankName=Пензенское отделение № 8624 ПАО 'Сбербанк России' г. Пенза|BIC=045655635|CorrespAcc=30101810000000000635|PayeeINN=6315376946|
Category=7|PersAcc={LIC}|LastName={FIO1.TryGetValue(0)}|FitstName={FIO1.TryGetValue(1)}|MiddleName={FIO1.TryGetValue(2)}
|PayerAddress={Lic.Street.Trim()}, дом {Lic.Home.Trim()}, кв. {Lic.Flat.Trim()}|Sum={sum1}";
                        generator1.Write(STR1).Save(path + $@"{LIC}_QRNew.png");
                        try
                        {
                            Microsoft.Office.Interop.Word.Range qr = doc.Range(0, 0);
                            qr.Find.Execute("{qr1}");
                            doc.Content.Find.Execute("{qr1}", false, true, false, false, false, true, 1, false, "", 2,
        false, false, false, false);
                            Range rangeImgQr = doc.Range(qr.Start, qr.End);
                            var ImgQr = doc.InlineShapes.AddPicture(path + $@"{LIC}_QRNew.png", false, true, rangeImgQr).ConvertToShape();
                            ImgQr.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapFront;

                        }
                        catch (Exception ex) { }
                    }
                    catch (Exception ex)
                    {
                        doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                           Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                           false);
                        app.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        app.Quit();
                    }
                    try
                    {

                        //doc.Save();
                        doc.ExportAsFixedFormat(path + $@"Квитанция ДПУ {LIC} {date.Month}.pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                        doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                           Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                           false);
                        app.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        app.Quit();
                    }
                    catch (Exception ex)
                    {
                        cacheApp.Update(LIC, $"Ошибка {ex.Message}");
                        app.Quit();
                    }
                    cacheApp.Update(LIC, $"Очищаю временные файлы квитанции");
                    if (File.Exists(path + $@"Образец квитанции DPUNew {LIC} {date.Month}.docx"))
                        File.Delete(path + $@"Образец квитанции DPUNew {LIC} {date.Month}.docx");
                    if (File.Exists(path + $@"{LIC}.png")) File.Delete(path + $@"{LIC}.png");
                    if (File.Exists(path + $@"{LIC}_QR.png")) File.Delete(path + $@"{LIC}_QR.png");
                    if (File.Exists(path + $@"{LIC}_QRNew.png")) File.Delete(path + $@"{LIC}_QRNew.png");
                    cacheApp.Delete(LIC);

                    return new PersDataDocumentLoad()
                    {
                        FileBytes = File.ReadAllBytes(path + $@"Квитанция ДПУ {LIC} {date.Month}.pdf"),
                        FileName = $@"Квитанция ДПУ {LIC} {date.Month}.pdf"
                    };
                }
                catch (Exception ex)
                {
                    cacheApp.AddProgress(LIC, ex.Message);
                    return new PersDataDocumentLoad()
                    {
                        FileBytes = new byte[0],
                        FileName = $@"Квитанция ДПУ {LIC} {date.Month}.pdf"
                    };
                }

            }
        }

        public string GenerateHtml(string LIC, DateTime date)
        {
            throw new NotImplementedException();
        }

        public string[] Substring(string T)
        {
            int z = 0;
            var arr = new string[4];
            if (T == null) return arr;
            for (int i = 0; i < T.Length - 1; i++)
            {
                if (i > 1000) break;
                if (i == 252 || i == 506 || i == 761)
                    z++;
                arr[z] += T[i];
            }
            return arr;
        }
    }
}
