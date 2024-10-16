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
using ClosedXML.Excel;
using System.Data;
using BL.Excel;
using System.Web;
using BL.Service;
using BL.Security;
using System.Threading.Tasks;
using DB.DataBase;
using BL.ApiT_;
using BL.Notification;
using System.Collections.Generic;
using DB.Model;
using static System.Net.WebRequestMethods;
using BL.Services;
using BE.Roles;
using System.Data.Entity;
using BL.Rules;
using BL.Services.FileServices;
using Microsoft.Ajax.Utilities;

namespace RKC.Controllers
{
    [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter + "," + RolesEnums.CounterReader)]
    public class CounterController : Controller
    {
        private readonly ICounter _counter;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly ICacheApp _cacheApp;
        private readonly IIntegrations _integration;
        private readonly IExcel _excel;
        private readonly IFlagsAction _flagsAction;
        private readonly IDictionary _dictionary;
        private readonly ISecurityProvider _securityProvider;
        private readonly IEBD _ebd;
        private readonly INotificationMail _notificationMail;
        private readonly IBaseService _baseService;
        private readonly ICounterFileServices _counterFileServices;
        private readonly NLog.Logger _Nlogger = NLog.LogManager.GetCurrentClassLogger();
        public CounterController(ICounter counter, Ilogger logger, IGeneratorDescriptons generatorDescriptons, 
            ICacheApp cacheApp, IFlagsAction flagsAction, IDictionary dictionary, IIntegrations integration
            , ISecurityProvider securityProvider,IEBD ebd,INotificationMail notificationMail, IExcel excel,
            IBaseService baseService, ICounterFileServices counterFileServices)
        {
            _excel = excel;
            _securityProvider = securityProvider;
            _counter = counter;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _dictionary = dictionary;
            _integration = integration;
            _ebd = ebd;
            _notificationMail = notificationMail;
            _baseService = baseService;
            _counterFileServices = counterFileServices;
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
        public async Task<ActionResult> DetailedInformIPU(string FULL_LIC)
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
                ViewBag.ZAK = _baseService.GetStatusCloseOpenLic(FULL_LIC);
                ViewBag.DIMENSION = await _dictionary.GetDIMENSION();
                ViewBag.Archive = await _dictionary.GetIpuArchiveReason();
                ViewBag.Recover = await _dictionary.GetIpuRecoverReason();
                ViewBag.LockAddPu = ViewBag.IsLock;
                if (ViewBag.IsLock == true && ViewBag.User == null) 
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
                var ttt = ex.InnerException;
                return Redirect("/Home/ResultEmpty?Message="+ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> DetailedInformIPUDelete(string FULL_LIC)
        {
            try
            {
                var Result = _counter.DetailInfroms(FULL_LIC,true);
                ViewBag.DIMENSION = await _dictionary.GetDIMENSION();
                ViewBag.Archive = await _dictionary.GetIpuArchiveReason();
                ViewBag.Recover = await _dictionary.GetIpuRecoverReason();
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
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter)]
        public async Task<ActionResult> FromAddPU(string FullLIC)
        {
            ViewBag.FULL_LIC = FullLIC;
            ViewBag.DIMENSION = await _dictionary.GetDIMENSION();
            return PartialView(_counter.GetTypeNowUsePU(FullLIC));
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter)]
        public ActionResult AddPU(ModelAddPU modelAddPU)
        {
            try
            {
                _Nlogger.Info(new ConvertJson<ModelAddPU>(modelAddPU).ConverModelToJson());
                SaveModelIPURules.Validation(modelAddPU);
                _baseService.CheckDublicateAddPuNumber(modelAddPU.FACTORY_NUMBER_PU, modelAddPU.TYPE_PU.GetDescription());
                if (_flagsAction.GetAction(nameof(DetailedInformIPU)))
                    return Redirect("home/ResultEmpty?Message=Невозможно добавить ИПУ программа заблокирована");
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
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter + "," + RolesEnums.CounterReader)]
        public ActionResult ArchiveAllLics(string id_pu)
        {
            ViewBag.FULL_LIC = id_pu.Split('-')[0];
            ViewBag.TypePU = id_pu.Split('-')[1];
            var Result = _counter.HistoryIndinikation(id_pu.Split('-')[0]);
            return PartialView(Result);
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter + "," + RolesEnums.CounterReader)]
        public ActionResult HistoryEdit(int id_pu)
        {
            return PartialView(_counter.HistoryEdit(id_pu));
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter)]
        public async Task<ActionResult> SaveIPU(SaveModelIPU saveModelIPU)
        {
            try
            {
                SaveModelIPURules.Validation(saveModelIPU);
                _Nlogger.Info(new ConvertJson<SaveModelIPU>(saveModelIPU).ConverModelToJson());
                _logger.ActionUsers(saveModelIPU.IdPU, _generatorDescriptons.Generate(saveModelIPU), User.Identity.GetFIOFull());
                await _counter.UpdateReadings(saveModelIPU);
                return Content("");
            }
            catch (Exception ex)
            {
                _Nlogger.Error(ex.Message);
                return Content($"Во время обновления произошла ошибка: {ex.Message}");
            }
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CounterWriter)]
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
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.RecoveryIpu)]
        public async Task<ActionResult> RecoveryPU(int IdPU, int RecoveryReasonId)
        {
            try
            {
                var reason = (await _dictionary.GetIpuRecoverReason()).FirstOrDefault(x => x.Id == RecoveryReasonId);
                _logger.ActionUsers(IdPU, $@"Востановил ПУ.
Изменили причину востановления: {reason.Name}", User.Identity.GetFIOFull());
                _counter.RecoveryIPU(IdPU, RecoveryReasonId);
                return Content("Востановление прошло успешно");
            }
            catch (Exception ex)
            {
                throw new Exception($"Во время востановления произошла ошибка {ex.Message}");
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
        [Auth(Roles = RolesEnums.Admin)]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file, string user, int TypeLoad)
        {
            return await _counterFileServices.UploadFile(file, user.IsNullOrWhiteSpace() ? User.Identity.Name : user, TypeLoad);
        }
        public ActionResult GetProgress(string Name)
        {
            return Content(_cacheApp.GetValueProgress(Name));
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin)]
        public async Task<ActionResult> Export(TypeFile typeFile, DateTime? dateTime)
        {
            return await _counterFileServices.Export(typeFile, dateTime, User.Identity.Name);
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin)]
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
        [Auth(Roles = RolesEnums.Admin)]
        public async Task<ActionResult> RunIntegration(DateTime date)
        {
            try
            {
                await _integration.LoadReadings("Integration", _cacheApp, date, _notificationMail, _counter);
            }
            catch (Exception ex)
            {
                return Redirect("home/ResultEmpty?Message=" + ex.Message);
            }
            return null;
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.SuperAdmin)]
        public async Task<ActionResult> RunIntegrationRepeat(DateTime date,string lic)
        {
            try
            {
                await _integration.LoadReadings("Integration", _cacheApp, date, _notificationMail, _counter,lic, date);
            }
            catch (Exception ex)
            {
                return Redirect("/home/ResultEmpty?Message=" + ex.Message);
            }
            return null;
        }
        public ActionResult ErrorIntegration()
        {
            return View(_integration.GetErrorIntegrationReadings().OrderByDescending(x => x.IsError));
        }
        [Auth(Roles = RolesEnums.Admin)]
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
        [Auth(Roles = "Admin")]
        public async Task<ActionResult> ErroIntegratinDelete(string Lic, string TypePU, int? Id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Result = new List<IntegrationReadings>();
                if (TypePU != "")
                    Result = db.IntegrationReadings.Where(x => x.Lic == Lic && x.TypePu == TypePU && x.IsError == true).ToList();
                if (TypePU == "")
                    Result = db.IntegrationReadings.Where(x => x.Lic == Lic && x.Description == "Не найден лицевой счет" && x.IsError == true).ToList();
                if(string.IsNullOrEmpty(TypePU) && Id.HasValue)
                {
                    var Integr = await db.IntegrationReadings.FirstOrDefaultAsync(x => x.Id == Id.Value );
                    Integr.IsError = false;
                    _ = await db.SaveChangesAsync();
                }
                foreach (var Items in Result)
                {
                    Items.IsError = false;
                   _ = await db.SaveChangesAsync();
                }
                return Redirect("/Counter/ErrorIntegration");
            }
        }
        [Auth(Roles = RolesEnums.SuperAdmin)]
        public ActionResult ExaminationPuIsLic()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workbook = new XLWorkbook();
                wb.Worksheets.Add(new CheckToExcel().PuIsLic(User.Identity.Name, _cacheApp));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет проверки приборов учета.xlsx");
                }
            }
        }
        public async Task<ActionResult> GetDictionatyOption(int? Id, string Text, string Type, string TypePU)
        {
            var Result = await _dictionary.GetDictionary(Id, Text, Type, TypePU);
            return PartialView("DictionatyOption", Result);
        }
    }
}