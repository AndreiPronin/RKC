using AppCache;
using BE.Court;
using BE.Roles;
using BL.Excel;
using BL.Helper;
using BL.Notification;
using BL.Security;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using RKC.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using CourtGeneralInformation = BE.Court.CourtGeneralInformation;
using CourtWorkRequisites = BE.Court.CourtWorkRequisites;
using DateTime = System.DateTime;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using InstallmentPayRequisites = BE.Court.InstallmentPayRequisites;
using LitigationWorkRequisites = BE.Court.LitigationWorkRequisites;

namespace RKC.Controllers
{
    [Auth(Roles = RolesEnums.CourtReader + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWriter + "," + RolesEnums.CourtSuperAdmin)]
    public class CourtController : Controller
    {
        private readonly ICourt _court;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly IDictionary _dictionary;
        private readonly ICacheApp _cacheApp;
        public readonly IFlagsAction _flagsAction;
        public readonly ISecurityProvider _securityProvider;
        private readonly IExcelCourt _excelCourt;
        private readonly INotificationMail _notificationMail;
        private readonly IPersonalData _personalData;
        private readonly IExcelCourtReport _excelCourtReport;
        private readonly NLog.Logger _Nlogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IExcel _excel;
        public CourtController(ICourt court, Ilogger logger, IGeneratorDescriptons generatorDescriptons, IDictionary dictionary,
            ICacheApp cacheApp, IFlagsAction flagsAction,
            ISecurityProvider securityProvider, IExcelCourt excelCourt, IPersonalData personalData , INotificationMail notificationMail, IExcelCourtReport excelCourtReport, 
            IExcel excel)
        {
            _securityProvider = securityProvider;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _dictionary = dictionary;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _court = court;
            _excelCourt = excelCourt;
            _personalData = personalData;
            _notificationMail = notificationMail;
            _excelCourtReport = excelCourtReport;
            _excel = excel;
        }
        [Auth(Roles = RolesEnums.CourtReader + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWriter + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> Index(int Id = 0)
        {
            if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(CourtController) + nameof(Index) + Id))
            {
                ViewBag.User = _cacheApp.GetValue(nameof(CourtController) + nameof(Index) + Id);
                ViewBag.IsLock = true;
            }
            else ViewBag.IsLock = false;

            

            var Model = await _court.DetailInfroms(Id);
            var debtInfoForLic = _personalData.GetDebtInfoForLic(Model.Lic);
            Model.CourtWork.SumDebtNowDate = debtInfoForLic is null ? Model.CourtWork.SumDebtNowDate : debtInfoForLic.CurrentDebt;
            return View(Model);
        }
        public ActionResult Serach()
        {
            return View();
        }
        public async Task<ActionResult> AdminPanel()
        {
            ViewBag.Dic = await _dictionary.GetAllCourtNameDictionaries();
            ViewBag.ReestyGPAccountingDepartment = _flagsAction.GetFlag(BE.Service.EnumFlags.ReestyGPAccountingDepartment);
            return View();
        }
        public async Task<ActionResult> SearchResult(SearchModel searchModel)
        {
            var Result = await _court.Serach(searchModel);
            return PartialView(Result);
        }
        [Auth(Roles = RolesEnums.CourtWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> CreateCourt(string FullLic)
        {
            _Nlogger.Trace($"Создал дело {FullLic}");
            if (string.IsNullOrEmpty(FullLic) || FullLic == "undefined")
                return Redirect("/Home/ResultEmpty?Message=" + "Нельзя указать пустой лицевой счет!");
            var Result = await _court.CreateCourt(FullLic, DateTime.Now.ToString(), User.Identity.GetFIOFull());
            return Redirect("/Court/Index?Id=" + Result);
        }
        [Auth(Roles = RolesEnums.SuperAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> DeleteCourt(int Id)
        {
            await _court.DeleteCourt(Id);
            return Redirect("/Court/Serach");
        }
        [Auth(Roles = RolesEnums.CourtWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> SaveCourt(CourtGeneralInformation courtGeneralInformation)
        {
            _Nlogger.Trace(new ConvertJson<CourtGeneralInformation>(courtGeneralInformation).ConverModelToJson());
            var Id = await _court.SaveCourt(courtGeneralInformation, User.Identity.GetFIOFull());
            return Redirect("/Court/Index?Id=" + Id);
        }
        [Auth(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWriter + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> ShowAllCourtModal(string FullLic)
        {
            ViewBag.FullLic = FullLic;
            var Result = await _court.GetAllCourtFullLic(FullLic);
            return PartialView(Result);
        }
        public async Task<JsonResult> AutocompleteDictionary(string Text, int Id)
        {
            var result = await _dictionary.GetCourtNameDictionaries(Text, Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetPartialDictionaryId(int Id)
        {
            var Result = await _dictionary.GetCourtValueDictionaryId(Id);
            return PartialView(Result);
        }
        public string GetActionUser(string Lic, int IdCourt)
        {
            return _logger.GetActionUserCourt(Lic, IdCourt);
        }
        public async Task<ActionResult> AddDicValue(int Id, string Value)
        {
            using (var AppDb = new ApplicationDbContext())
            {
                AppDb.CourtValueDictionary.Add(new DB.Model.Court.DictiomaryModel.CourtValueDictionary
                {
                    CourtNameDictionaryId = Id,
                    Name = Value,
                });
                await AppDb.SaveChangesAsync();
            }
            return null;
        }
        public async Task<ActionResult> AddNewDic(string Value)
        {
            using (var AppDb = new ApplicationDbContext())
            {
                AppDb.CourtNameDictionaries.Add(new DB.Model.Court.DictiomaryModel.CourtNameDictionary
                {
                    Name = Value,
                });
                await AppDb.SaveChangesAsync();
            }
            return null;
        }
        public async Task<ActionResult> AddCourtWorkRequisites(CourtWorkRequisites courtWorkRequisites)
        {
            _Nlogger.Trace($"{nameof(AddCourtWorkRequisites)} {new ConvertJson<CourtWorkRequisites>(courtWorkRequisites).ConverModelToJson()}");
            await _court.AddCourtWorkRequisites(courtWorkRequisites);
            return Content("Успешно добавлено");
        }
        public async Task<ActionResult> PartialViewCourtWorkRequisites(int Id)
        {
            var res = await _court.GetCourtWorkRequisites(Id);
            return PartialView(res);
        }
        public async Task<ActionResult> AddLitigationWorkRequisites(LitigationWorkRequisites litigationWorkRequisites)
        {
            _Nlogger.Trace($"{nameof(AddLitigationWorkRequisites)} {new ConvertJson<LitigationWorkRequisites>(litigationWorkRequisites).ConverModelToJson()}");
            await _court.AddLitigationWorkRequisites(litigationWorkRequisites);
            return Content("Успешно добавлено");
        }
        public async Task<ActionResult> RemoveLitigationWorkRequisites(int Id)
        {
            await _court.RemoveLitigationWorkRequisites(Id);
            return Content("Успешно удалено");
        }
        public async Task<ActionResult> PartialViewLitigationWorkRequisites(int Id)
        {
            var res = await _court.GetLitigationWorkRequisites(Id);
            return PartialView(res);
        }
        public async Task<ActionResult> RemoveCourtWorkRequisites(int Id)
        {
            await _court.RemoveCourtWorkRequisites(Id);
            return Content("Успешно удалено");
        }
        public async Task<ActionResult> AddInstallmentPayRequisites(InstallmentPayRequisites installmentPayRequisites)
        {
            _Nlogger.Trace($"{nameof(AddInstallmentPayRequisites)}  {new ConvertJson<InstallmentPayRequisites>(installmentPayRequisites).ConverModelToJson()}");
            await _court.AddInstallmentPayRequisites(installmentPayRequisites);
            return Content("Успешно добавлено");
        }
        public async Task<ActionResult> RemoveInstallmentPayRequisites(int Id)
        {
            await _court.RemoveInstallmentPayRequisites(Id);
            return Content("Успешно удалено");
        }

        public async Task<ActionResult> PartialViewInstallmentPayRequisites(int Id)
        {
            var res = await _court.GetInstallmentPayRequisites(Id);
            return PartialView(res);
        }

        public async Task<ActionResult> PartialViewGetAllFilesInCourt(int Id)
        {
            var Res = await _court.GetDocumentScans(Id);
            return PartialView(Res);
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWriter + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> SaveFile(HttpPostedFileBase FileLoad, string NameFile, string Lic, int CourtId)
        {
            return Json(new
            {
                result = await _court.saveFile(ConverToBytes(FileLoad), CourtId, Lic, FileLoad.FileName.Split('.').LastOrDefault(), NameFile, User.Identity.GetFIOFull()),
                JsonRequestBehavior.AllowGet
            });
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWriter + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public async Task<ActionResult> DownLoadFile(int Id)
        {
            var Result = await _court.DownLoadFile(Id);

            return File(Result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Result.FileName);
        }
        [HttpPost]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWriter + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin)]
        public ActionResult SaveNote([FromBody]string Note, [FromUri]int Id, [FromUri] string Lic)
        {
            _court.SaveNote(Note,Id,Lic);

            return Content("Ok");
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWriter + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtAdmin + "," + RolesEnums.CourtReader + "," + RolesEnums.CourtSuperAdmin)]
        public string GetNote([FromUri] int Id, [FromUri] string Lic)
        {
            var result = _court.GetNote(Id, Lic);

            return result;
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWriter + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DeleteFile(int Id)
        {
            await _court.DeleteFile(Id, User.Identity.GetFIOFull());
            return null;
        }
        public static byte[] ConverToBytes(HttpPostedFileBase file)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
        [HttpGet]
        public ActionResult clearCache(string Page)
        {
            _cacheApp.Delete(User.Identity.GetFIOFull(), Page);
            return null;
        }
        [Auth(Roles = RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> UploadFileCourtCase(HttpPostedFileBase file, int TypeLoad)
        {
            _Nlogger.Info($"Загружает шаблон {TypeLoad} название файла {file.FileName}");
            if (string.IsNullOrEmpty(_cacheApp.GetValue(nameof(UploadFileCourtCase)))){
                _cacheApp.LockUpload(nameof(UploadFileCourtCase), $"{User.Identity.GetFIOFull()} {file.FileName}",true);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    try
                    {
                        var workbook = new XLWorkbook(file.InputStream);
                        switch ((CourtTypeLoadFiles)TypeLoad)
                        {

                            case CourtTypeLoadFiles.OpenNewCourt:
                                wb.Worksheets.Add(await _excelCourt.ExcelsLoadCourt(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат загрузки.xlsx");
                                }
                            case CourtTypeLoadFiles.EditGp:
                                wb.Worksheets.Add(await _excelCourt.ExcelsEditGpCourt(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления ГП.xlsx");
                                }
                            case CourtTypeLoadFiles.EditPersData:
                                wb.Worksheets.Add(await _excelCourt.ExcelsEditPersDataCourt(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления перс данных.xlsx");
                                }
                            case CourtTypeLoadFiles.EditSpAndIp:
                                wb.Worksheets.Add(await _excelCourt.ExcelsEditSpAndIpCourt(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete( $"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Изменение СП и ИП.xlsx");
                                }
                            case CourtTypeLoadFiles.EditOwner:
                                wb.Worksheets.Add(await _excelCourt.ExcelsEditOwnerCourt(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Изменение Собственника.xlsx");
                                }
                            case CourtTypeLoadFiles.UpdateNote:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadNote(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления примечания.xlsx");
                                }
                            case CourtTypeLoadFiles.InstallmentPlan:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadInstallmentPlan(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Рассрочки.xlsx");
                                }
                            case CourtTypeLoadFiles.Bankruptcy:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadBankruptcy(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Банкротсво.xlsx");
                                }
                            case CourtTypeLoadFiles.WriteOff:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadWriteOff(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Списание.xlsx");
                                }
                            case CourtTypeLoadFiles.OpenLitigationWork:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadOpenLitigationWork(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Списание.xlsx");
                                }
                            case CourtTypeLoadFiles.LitigationWork:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadLitigationWork(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Списание.xlsx");
                                }
                            case CourtTypeLoadFiles.EnteringDecision:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadEnteringDecision(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Списание.xlsx");
                                }
                            case CourtTypeLoadFiles.PdFromIp:
                                wb.Worksheets.Add(await _excelCourt.ExcelsDownloadPdFromIp(workbook, $"{User.Identity.GetFIOFull()} {file.FileName}"));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Результат обновления Списание.xlsx");
                                }
                            default:
                                throw new Exception("Не указан тип загружаемого файла! (Судебные дела)");
                        }
                    } catch (Exception ex)
                    {
                        _cacheApp.Delete($"{User.Identity.GetFIOFull()} {file.FileName}", nameof(UploadFileCourtCase));
                        _notificationMail.Error(ex, "Ошибка во время прогрузки судебных дел  (Судебные дела)");
                        return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                    }
                }
            }
            else
            {
                return Redirect("/Home/ResultEmpty?Message=Загрузка заблокирована дождитесь окончания загрузки (Судебные дела)");
            }
        }
        [Auth(Roles = RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin +"," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> Report(CourtTypeReport courtTypeReport, DateTime? date)
        {
            _Nlogger.Info($"Выгрузил отчет {courtTypeReport.GetDescription()} Пользователь {User.Identity.Name}");
            if (string.IsNullOrEmpty(_cacheApp.GetValue(nameof(Report))))
            {
                _cacheApp.LockUpload(nameof(Report), $"{User.Identity.GetFIOFull()} {(int)courtTypeReport}", true);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    try
                    {
                        var workbook = new XLWorkbook();
                        switch (courtTypeReport)
                        {

                            case CourtTypeReport.ReestyGPAccountingDepartment:
                                wb.Worksheets.Add(await _excelCourtReport.ReestyGPAccountingDepartment(workbook, $"{User.Identity.GetFIOFull()} {(int)courtTypeReport}", 
                                    courtTypeReport, date.Value));
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    _cacheApp.Delete($"{User.Identity.GetFIOFull()} {(int)courtTypeReport}", nameof(Report));
                                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                                        $"{courtTypeReport.GetDescription()} - {date.Value.ToString("dd-MM-yyyy")}.xlsx");
                                }
                            default:
                                throw new Exception("Не указан тип загружаемого отчета! (Судебные дела)");
                        }
                    }
                    catch (Exception ex)
                    {
                        _cacheApp.Delete($"{User.Identity.GetFIOFull()} {(int)courtTypeReport}", nameof(Report));
                        _notificationMail.Error(ex, "Ошибка во время загрузки отчета судебных дел (Судебные дела)");
                        return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                    }
                }
            }
            else
            {
                return Redirect("/Home/ResultEmpty?Message=Загрузка отчетов заблокирована дождитесь окончания загрузки (Судебные дела)");
            }
        }
        [Auth(Roles = RolesEnums.CourtAdmin + "," + RolesEnums.CourtSuperAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> Report2(CourtTypeReport2 courtTypeReport)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    switch (courtTypeReport)
                    {

                        case CourtTypeReport2.LoadIP:
                            var wb = _excel.ExcelReportFunction(workbook, BL.Excel.SqlQuery.Report.IPUpload, 22, "U");

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{courtTypeReport.GetDescription()} - {DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
                            }
                        case CourtTypeReport2.ExecutorSP:
                            var wb1 = _excel.ExcelReportFunction(workbook, BL.Excel.SqlQuery.Report.UploadExecutionSP, 22, "U");

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb1.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{courtTypeReport.GetDescription()} - {DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
                            }
                        case CourtTypeReport2.TaskGPH:
                            var wb2 = _excel.ExcelReportFunction(workbook, BL.Excel.SqlQuery.Report.TaskForGPH, 22, "U");

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb2.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{courtTypeReport.GetDescription()} - {DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
                            }
                        case CourtTypeReport2.NowDZ:
                            var wb3 = _excel.ExcelReportFunction(workbook, BL.Excel.SqlQuery.Report.CurrentDebt, 22, "J");

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb3.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{courtTypeReport.GetDescription()} - {DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
                            }
                        case CourtTypeReport2.WriteOff:
                            var wb4 = await _excelCourtReport.WriteOff(workbook, User.Identity.GetFIOFull(), courtTypeReport, DateTime.Now);

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb4.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{courtTypeReport.GetDescription()} - {DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
                            }
                        default:
                            throw new Exception("Не указан тип загружаемого отчета! (Судебные дела)");
                    }
                }
            }
            catch (Exception ex)
            {
                _notificationMail.Error(ex, "Ошибка во время загрузки отчета судебных дел (Судебные дела)");
                return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
            }
        }
    }
}

