using AppCache;
using BE.Counter;
using BL.ApiT_;
using BL.Excel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BL.Services.FileServices
{
    public interface ICounterFileServices
    {
        Task<ActionResult> UploadFile(HttpPostedFileBase file, string User, int TypeLoad);
        Task<ActionResult> Export(TypeFile typeFile, DateTime? dateTime);
    }
    public class CounterFileServices :  Controller , ICounterFileServices
    {
        private readonly IExcel _excel;
        private readonly ICacheApp _cacheApp;
        private readonly IEBD _ebd;
        public CounterFileServices(IExcel excel, ICacheApp cacheApp, IEBD ebd) 
        {
            _excel = excel;
            _cacheApp = cacheApp;
            _ebd = ebd;
        }
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file, string User, int TypeLoad)
        {
            if (TypeLoad == 1)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelNewPUProperty(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 2)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelPUProperty(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 3)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelSquarePersProperty(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 4)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelNewPersonalData(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 5)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelUpdatePersonalDataMain(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 6)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(await _excel.MassClosePU(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 7)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.LoadExcelUpdatePersonalDataMainFio(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 8)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(await _excel.LoadExcelArrayCloseLicAsync(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if (TypeLoad == 9)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(_excel.OpenNewPuWithIndications(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            return null;
        }
        public async Task<ActionResult> Export(TypeFile typeFile, DateTime? dateTime)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dateTime.HasValue && (typeFile.Equals(TypeFile.EbdAll) || typeFile.Equals(TypeFile.EbdMkd)
                    || typeFile.Equals(TypeFile.EbdFlatliving) || typeFile.Equals(TypeFile.EbdFlatNotliving)))
                    _ebd.UpdateLastLoadEbd(dateTime.Value);
                if (typeFile.Equals(TypeFile.EbdAll))
                {
                    return File(_ebd.CreateEBDAll(dateTime.Value), "application/octet-stream", $"{TypeFile.EbdAll.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.EbdMkd))
                {
                    return File(_ebd.CreateEbdMkd(dateTime.Value), "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.EbdFlatliving))
                {
                    return File(_ebd.CreateEbdFlatliving(dateTime.Value), "application/octet-stream", $"{TypeFile.EbdFlatliving.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.EbdFlatNotliving))
                {
                    return File(_ebd.CreateEbdFlatNotliving(dateTime.Value), "application/octet-stream", $"{TypeFile.EbdFlatNotliving.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.DirectFlat))
                {
                    return File(_ebd.CreateDirectFlat(), "application/octet-stream", $"{TypeFile.DirectFlat.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.DirectMkd))
                {
                    return File(_ebd.CreateDirectMkd(), "application/octet-stream", $"{TypeFile.DirectMkd.GetDescription()}.xml");
                }
                if (typeFile.Equals(TypeFile.Counters))
                {
                    wb.Worksheets.Add(_excel.CreateExcelCounters());
                }
                if (typeFile.Equals(TypeFile.Lic))
                {
                    wb.Worksheets.Add(await _excel.CreateExcelLic(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.LogPers))
                {
                    wb.Worksheets.Add(await _excel.CreateExcelLogPers());
                }
                if (typeFile.Equals(TypeFile.LogCounter))
                {
                    wb.Worksheets.Add(await _excel.CreateExcelLogCounter());
                }
                if (typeFile.Equals(TypeFile.TIpuOtp))
                {
                    wb.Worksheets.Add(_excel.TIpuOtp(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.TIpuGvs))
                {
                    wb.Worksheets.Add(_excel.TIpuGvs(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.SummaryReportOTP))
                {
                    var result = _excel.SummaryReportOTP(wb, User.Identity.Name, _cacheApp);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        result.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile) + ".xlsx");
                    }
                }
                if (typeFile.Equals(TypeFile.SummaryReportGVS))
                {
                    var result = _excel.SummaryReportGVS(wb, User.Identity.Name, _cacheApp);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        result.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile) + ".xlsx");
                    }
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile) + ".xlsx");
                }
            }
            // 
        }
    }
}
