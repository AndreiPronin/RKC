using BE.Counter;
using BL.Counters;
using System;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using RKC.Extensions;
using BL.Helper;
using AppCache;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Data;
using BL.Excel;
using System.Web;
using System.Data.Odbc;
using BL.Service;
using BL;
using BL.Security;
using System.Threading.Tasks;
using DB.DataBase;
using BL.ApiT_;
using BL.Notification;
using System.Collections.Generic;
using DB.Model;
using static System.Net.WebRequestMethods;

namespace RKC.Controllers
{
    public class CounterController : Controller
    {
        private readonly ICounter _counter;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly ICacheApp _cacheApp;
        private readonly IIntegrations _integration;
        private readonly IExcel _excel;
        public readonly IFlagsAction _flagsAction;
        public readonly IReadFileBank _readFileBank;
        public readonly ISecurityProvider _securityProvider;
        public readonly IEBD _ebd;
        public readonly INotificationMail _notificationMail;
        public CounterController(ICounter counter, Ilogger logger, IGeneratorDescriptons generatorDescriptons, 
            ICacheApp cacheApp, IFlagsAction flagsAction, IReadFileBank readFileBank, IIntegrations integration
            , ISecurityProvider securityProvider,IEBD ebd,INotificationMail notificationMail, IExcel excel)
        {
            _excel = excel;
            _securityProvider = securityProvider;
            _counter = counter;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _readFileBank = readFileBank;
            _integration = integration;
            _ebd = ebd;
            _notificationMail = notificationMail;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchIPU(SearchIPU_LICModel searchModel)
        {
            var Result = _counter.SearchIPU_LIC(searchModel);
            ViewBag.Count = Result.Count();
            ViewBag.Count = ViewBag.Count == 0 ? "Ничего не найдено" : $"Найдено {ViewBag.Count} записей";
            return PartialView(Result);
        }
        [HttpGet]
        public ActionResult DetailedInformIPU(string FULL_LIC)
        {
            try
            {
                ViewBag.ErrorIntegration = _integration.GetErrorIntegrationReadings(FULL_LIC); 
                if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(DetailedInformIPU) + FULL_LIC))
                {
                    ViewBag.User = _cacheApp.GetValue(nameof(DetailedInformIPU) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                if (ViewBag.IsLock == false)
                {
                    ViewBag.IsLock = _flagsAction.GetAction(nameof(DetailedInformIPU));
                }
                _counter.AutoAddPU(FULL_LIC);
                var Result = _counter.DetailInfroms(FULL_LIC);
                if(ViewBag.IsLock == true && ViewBag.User == null) 
                    ViewBag.IsLock = _securityProvider.GetRoleUserNoLock(User.Identity.GetUserId());
                if (Result.Count() > 0)
                {
                    return View(Result);
                }
                else
                {
                    
                    Result = _counter.DetailInfroms(FULL_LIC);
                    if (Result.Count() == 0)
                    {
                        ViewBag.FULL_LIC = FULL_LIC;
                    }
                    return View(Result); 
                }
            }
            catch (Exception ex) {
                return Redirect("/Home/ResultEmpty?Message="+ex.Message);
            }
        }
        [HttpGet]
        public ActionResult DetailedInformIPUDelete(string FULL_LIC)
        {
            try
            {
                var Result = _counter.DetailInfromsDelete(FULL_LIC);

                if (Result.Count() > 0)
                {
                    ViewBag.FULL_LIC = FULL_LIC;
                }
                return View(Result);
            }
            catch (Exception ex)
            {
                return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,Admin")]
        public ActionResult FromAddPU(string FullLIC)
        {
            ViewBag.FULL_LIC = FullLIC;
            return PartialView(_counter.GetTypeNowUsePU(FullLIC));
        }
        [HttpPost]
        [Authorize(Roles = "CounterWriter,Admin")]
        public ActionResult AddPU(ModelAddPU modelAddPU)
        {
            try
            {
                if (string.IsNullOrEmpty(modelAddPU.FULL_LIC) || modelAddPU.FULL_LIC == "undefined")
                {
                    throw new Exception("Не найден лицевой счет, обратитесь к администратору");
                }
                _counter.AddPU(modelAddPU, User.Identity.GetFIOFull());
                return null;
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult clearCache(string Page)
        {
            _cacheApp.Delete(User.Identity.GetFIOFull(), Page);
            return null;
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,CounterReader,Admin")]
        public ActionResult ArchiveAllLics(string id_pu)
        {
            ViewBag.FULL_LIC = id_pu.Split('-')[0];
            ViewBag.TypePU = id_pu.Split('-')[1];
            var Result = _counter.HistoryIndinikation(id_pu.Split('-')[0]);
            return PartialView(Result);
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,CounterReader,Admin")]
        public ActionResult HistoryEdit(int id_pu)
        {
            return PartialView(_counter.HistoryEdit(id_pu));
        }
        [HttpPost]
        [Authorize(Roles = "CounterWriter,Admin")]
        public ActionResult SaveIPU(SaveModelIPU saveModelIPU)
        {
            try
            {
                _logger.ActionUsers(saveModelIPU.IdPU, _generatorDescriptons.Generate(saveModelIPU), User.Identity.GetFIOFull());
                _counter.UpdateReadings(saveModelIPU);
                return Content("");
            }
            catch (Exception ex)
            {
                return Content($"Во время обновления произошла ошибка {ex.Message}");
            }
        }
        [HttpPost]
        [Authorize(Roles = "CounterWriter,Admin")]
        public ActionResult DeletePU(int IdPU)
        {
            try
            {
                _logger.ActionUsers(IdPU, "Удалил ПУ", User.Identity.GetFIOFull());
                _counter.DeleteIPU(IdPU);
                return Content("Удаление прошло успешно");
            }
            catch (Exception ex)
            {
                return Content($"Во время удаления произошла ошибка {ex.Message}");
            }
        }
        public ActionResult DeleteError(int IdPU, string Lic)
        {
            try
            {
                _counter.DeleteError(IdPU, Lic, User.Identity.GetFIOFull());
                return Content("Удаление прошло успешно");
            }
            catch (Exception ex)
            {
                return Content($"Во время удаления произошла ошибка {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UploadFile(HttpPostedFileBase file, Banks Bank)
        {
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                byte[] fileData = binaryReader.ReadBytes(file.ContentLength);
                var Result = _readFileBank.Read(fileData, Bank);
                return View(Result);
            }

        }
        [Authorize(Roles = "Admin")]
        public ActionResult UploadFilePU(HttpPostedFileBase file, string User, int TypeLoad)
        {
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
                    wb.Worksheets.Add(_excel.MassClosePU(workbook, User, _cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            return null;
        }
        public ActionResult GetProgress(string Name)
        {
            return Content(_cacheApp.GetValueProgress(Name));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Export(TypeFile typeFile, DateTime? dateTime)
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
                if (typeFile.Equals(TypeFile.Counters))
                {
                    wb.Worksheets.Add(_excel.CreateExcelCounters());
                }
                if (typeFile.Equals(TypeFile.Lic))
                {
                    wb.Worksheets.Add(_excel.CreateExcelLic(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.General))
                {
                    wb.Worksheets.Add(_excel.CreateExcelGeneral());
                }
                if (typeFile.Equals(TypeFile.ReestrIPU))
                {
                    wb.Worksheets.Add(_excel.ReestrIPU(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.TIpuOtp))
                {
                    wb.Worksheets.Add(_excel.TIpuOtp(User.Identity.Name, _cacheApp));
                }
                if (typeFile.Equals(TypeFile.TIpuGvs))
                {
                    wb.Worksheets.Add(_excel.TIpuGvs(User.Identity.Name, _cacheApp));
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile) + ".xlsx");
                }
            }
            // 
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult LoadTemplate(TypeTemplateFile typeFile)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workbook = new XLWorkbook();
                if (typeFile.Equals(TypeTemplateFile.LoadExcelPUProperty))
                {
                    wb.Worksheets.Add(new ExcelTemplate().LoadExcelPUProperty());
                }
                if (typeFile.Equals(TypeTemplateFile.LoadExcelPersData))
                {
                    wb.Worksheets.Add(new ExcelTemplate().LoadExcelPUProperty());
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{TypeTemplateFile.LoadExcelPUProperty.GetDescription()}.xlsx");
                }
            }
            // 
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RunIntegration(DateTime date)
        {
            try
            {
                await _integration.LoadReadings("Integration", _cacheApp, date, _notificationMail);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public ActionResult ErrorIntegration()
        {
            return View(_integration.GetErrorIntegrationReadings().OrderByDescending(x => x.IsError));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ErroIntegratinLoadExcel()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_excel.ErroIntegratin());
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Интеграция .xlsx");
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ErroIntegratinDelete(string Lic, string TypePU)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Result = new List<IntegrationReadings>();
                if (TypePU != "")
                    Result = db.IntegrationReadings.Where(x => x.Lic == Lic && x.TypePu == TypePU && x.IsError == true).ToList();
                if (TypePU == "")
                    Result = db.IntegrationReadings.Where(x => x.Lic == Lic && x.Description == "Не найден лицевой счет" && x.IsError == true).ToList();
                foreach (var Items in Result)
                {
                    Items.IsError = false;
                    await db.SaveChangesAsync();
                }
                return Redirect("/Counter/ErrorIntegration");
            }
        }
    }
}