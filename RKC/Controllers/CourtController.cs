using AppCache;
using BE.Court;
using BE.Roles;
using BL.Excel;
using BL.Helper;
using BL.Security;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using Microsoft.AspNet.Identity;
using RKC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;

namespace RKC.Controllers
{
    [Auth(Roles = RolesEnums.CourtReader + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWhriter)]
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
        public CourtController(ICourt court, Ilogger logger, IGeneratorDescriptons generatorDescriptons, IDictionary dictionary,
            ICacheApp cacheApp, IFlagsAction flagsAction,
            ISecurityProvider securityProvider, IExcelCourt excelCourt)
        {
            _securityProvider = securityProvider;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _dictionary = dictionary;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _court = court;
            _excelCourt = excelCourt;
        }
        public async Task<ActionResult> Index(int Id = 0)
        {
            if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(CourtController) + nameof(Index) + Id))
            {
                ViewBag.User = _cacheApp.GetValue(nameof(CourtController) + nameof(Index) + Id);
                ViewBag.IsLock = true;
            }
            else ViewBag.IsLock = false;

            var Model = await _court.DetailInfroms(Id);
            return View(Model);
        }
        public ActionResult Serach()
        {
            return View();
        }
        public async Task<ActionResult> AdminPanel()
        {
            ViewBag.Dic = await _dictionary.GetAllCourtNameDictionaries();
            return View();
        }
        public async Task<ActionResult> SearchResult(SearchModel searchModel)
        {
            var Result = await _court.Serach(searchModel);
            return PartialView(Result);
        }
        [Auth(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> CreateCourt(string FullLic)
        {
            if (string.IsNullOrEmpty(FullLic) || FullLic == "undefined")
                return Redirect("/Home/ResultEmpty?Message=" + "Нельзя указать пустой лицевой счет!");
            var Result = await _court.CreateCourt(FullLic, DateTime.Now.ToString(), User.Identity.GetFIOFull());
            return Redirect("/Court/Index?Id=" + Result);
        }
        public async Task<ActionResult> DeleteCourt(int Id)
        {
            await _court.DeleteCourt(Id);
            return Redirect("/Court/Serach");
        }
        [Auth(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> SaveCourt(CourtGeneralInformation courtGeneralInformation)
        {
            var Id = await _court.SaveCourt(courtGeneralInformation, User.Identity.GetFIOFull());
            return Redirect("/Court/Index?Id=" + Id);
        }
        [Auth(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWhriter)]
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
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWhriter + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> SaveFile(HttpPostedFileBase FileLoad, string NameFile, string Lic, int CourtId)
        {
            return Json(new
            {
                result = await _court.saveFile(ConverToBytes(FileLoad), CourtId, Lic, FileLoad.FileName.Split('.').LastOrDefault(), NameFile, User.Identity.GetFIOFull()),
                JsonRequestBehavior.AllowGet
            });
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWhriter + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> DownLoadFile(int Id)
        {
            var Result = await _court.DownLoadFile(Id);

            return File(Result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Result.FileName);
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.CourtWhriter + "," + RolesEnums.SuperAdmin)]
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
        [Auth(Roles = RolesEnums.CourtAdmin)]
        public async Task<ActionResult> UploadFileCourtCase(HttpPostedFileBase file)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                try
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(await _excelCourt.ExcelsLoadCourt(workbook));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }catch (Exception ex)
                {
                    return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                }
            }
        }
    }
}

