using BE.DPU;
using BE.PersData;
using BE.Roles;
using BL.Excel;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
using DB.Query;
using DocumentFormat.OpenXml.Drawing.Charts;
using Ionic.Zip;
using RKC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WordGenerator;
using WordGenerator.Enums;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace RKC.Controllers
{

    [Auth(Roles = RolesEnums.DPUReader + "," + RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
    public class DPUController : Controller
    {
        private readonly IExcelDpu _excelDpu;
        private readonly IDpu _dpu;
        private readonly IPdfFactory _pdfFactory;

        public DPUController(IExcelDpu excelDpu, IDpu dpu, IPdfFactory pdfFactory)
        {
            _excelDpu = excelDpu;
            _dpu = dpu;
            _pdfFactory = pdfFactory;
        }

        // GET: DPU
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> SearchAutocompleteDpu(string Text)
        {
            var result = await _dpu.SearchAutocompleteDPU(Text);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Auth(Roles = RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> UploadFilePU(HttpPostedFileBase file, int TypeLoad)
        {
            if (TypeLoad == 1)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(await _excelDpu.LoadDPUHelpCalculationInstallation(workbook));
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
                    wb.Worksheets.Add(await _excelDpu.LoadDPUSummaryHouses(workbook));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            return null;
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.DPUReader + "," + RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DpuInfo(int id)
        {
            return PartialView(await _dpu.GetDpu(id));
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.DPUReader + "," + RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DpuDictionarySummaryHouses()
        {
            return PartialView(await _dpu.GetDPUSummaryHouses());
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DpuSaveNote([FromBody]int? Id, [FromBody] string Note)
        {
            await _dpu.DpuSaveNote(Id.Value, Note);
            return null;
        }
        
        [HttpGet]
        [Auth(Roles = RolesEnums.DPUReader + "," + RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> WatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var Result = await _dpu.GetWatchHelpСalculation(FullLic, DateFrom, DateTo);
                return View(Result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.DPUReader + "," + RolesEnums.DPUEdit + "," + RolesEnums.DPUAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var Result = await _dpu.DownLoadHelpСalculation(FullLic, DateFrom, DateTo);
                return File(Result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Result.FileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Auth(Roles = "Admin,DownLoadReceipt")]
        [HttpGet]
        public ActionResult DownLoadReceipt(string FullLic, DateTime DateStart, DateTime DateEnd)
        {
            if (DateStart == DateEnd)
            {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var Dpu = db.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationViewPeriodExhibid).FirstOrDefault(x => x.NewFullLic == FullLic && x.Period.Year == DateEnd.Year && x.Period.Month == DateEnd.Month);
                        if(Dpu == null || Dpu?.Period == Dpu?.PeriodExhibid) { 
                            var result = _pdfFactory.CreatePdf(PdfType.NewDpu).Generate(FullLic, DateEnd);
                            return File(result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.FileName);
                        }
                        else
                        {
                            var result = _pdfFactory.CreatePdf(PdfType.Dpu).Generate(FullLic, DateEnd);
                            return File(result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                }
            }
            List<PersDataDocumentLoad> persData = new List<PersDataDocumentLoad>();
            while (DateStart >= DateEnd)
            {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var Dpu = db.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationViewPeriodExhibid).FirstOrDefault(x => x.NewFullLic == FullLic && x.Period.Year == DateEnd.Year && x.Period.Month == DateEnd.Month);
                        if (Dpu == null || Dpu?.Period == Dpu?.PeriodExhibid)
                        {
                            persData.Add(_pdfFactory.CreatePdf(PdfType.NewDpu).Generate(FullLic, DateEnd));
                        }
                        else
                        {
                            persData.Add(_pdfFactory.CreatePdf(PdfType.Dpu).Generate(FullLic, DateEnd));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                DateEnd = DateEnd.AddMonths(1);
            }

            MemoryStream outputStream = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                foreach (var Items in persData)
                {
                    var entry = zip.CreateEntry(Items.FileName);

                    try
                    {
                        using (Stream stream = entry.Open())
                        {
                            stream.Write(Items.FileBytes, 0, Items.FileBytes.Length);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            outputStream.Position = 0;
            return File(outputStream, MediaTypeNames.Application.Octet, "Квитанция.zip");
        }
    }
   
}