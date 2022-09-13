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

namespace RKC.Controllers
{
    [Authorize]
    public class CounterController : Controller
    {
        private readonly ICounter counter;
        private readonly Ilogger logger;
        private readonly IGeneratorDescriptons generatorDescriptons;
        private readonly ICacheApp cacheApp;
        private readonly IIntegrations _integration;
        public readonly IFlagsAction flagsAction;
        public readonly IReadFileBank readFileBank;
        public readonly ISecurityProvider _securityProvider;
        public readonly IEBD _ebd;
        public CounterController(ICounter _counter, Ilogger _logger, IGeneratorDescriptons _generatorDescriptons, 
            ICacheApp _cacheApp, IFlagsAction _flagsAction, IReadFileBank _readFileBank, IIntegrations integration
            , ISecurityProvider securityProvider,IEBD ebd)
        {
            _securityProvider = securityProvider;
            counter = _counter;
            logger = _logger;
            generatorDescriptons = _generatorDescriptons;
            cacheApp = _cacheApp;
            flagsAction = _flagsAction;
            readFileBank = _readFileBank;
            _integration = integration;
            _ebd = ebd;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchIPU(SearchIPU_LICModel searchModel)
        {
            var Result = counter.SearchIPU_LIC(searchModel);
            ViewBag.Count = Result.Count();
            ViewBag.Count = ViewBag.Count == 0 ? "Ничего не найдено" : $"Найдено {ViewBag.Count} записей";
            return PartialView(Result);
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,CounterReader,Admin")]
        public ActionResult DetailedInformIPU(string FULL_LIC)
        {
            try
            {
                ViewBag.ErrorIntegration = _integration.GetErrorIntegrationReadings(FULL_LIC); 
                if (cacheApp.Lock(User.Identity.GetFIOFull(), nameof(DetailedInformIPU) + FULL_LIC))
                {
                    ViewBag.User = cacheApp.GetValue(nameof(DetailedInformIPU) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                if (ViewBag.IsLock == false)
                {
                    ViewBag.IsLock = flagsAction.GetAction(nameof(DetailedInformIPU));
                }
                var Result = counter.DetailInfroms(FULL_LIC);
                if(ViewBag.IsLock == true) ViewBag.IsLock = _securityProvider.GetRoleUserNoLock(User.Identity.GetUserId());
                if (Result.Count() > 0)
                {
                    return View(Result);
                }
                else
                {
                    counter.AutoAddPU(FULL_LIC);
                    Result = counter.DetailInfroms(FULL_LIC);
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
                var Result = counter.DetailInfromsDelete(FULL_LIC);

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
            return PartialView(counter.GetTypeNowUsePU(FullLIC));
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
                counter.AddPU(modelAddPU,User.Identity.GetFIOFull());
                return null;
            }catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult clearCache(string Page)
        {
            cacheApp.Delete(User.Identity.GetFIOFull(),Page);
            return null;
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,CounterReader,Admin")]
        public ActionResult ArchiveAllLics(string id_pu)
        {
            ViewBag.FULL_LIC = id_pu.Split('-')[0];
            ViewBag.TypePU = id_pu.Split('-')[1];
            var Result = counter.HistoryIndinikation(id_pu.Split('-')[0]);
            return PartialView(Result);
        }
        [HttpGet]
        [Authorize(Roles = "CounterWriter,CounterReader,Admin")]
        public ActionResult HistoryEdit(int id_pu)
        {
            return PartialView(counter.HistoryEdit(id_pu));
        }
        [HttpPost]
        [Authorize(Roles = "CounterWriter,Admin")]
        public ActionResult SaveIPU(SaveModelIPU saveModelIPU)
        {
            try
            {
                logger.ActionUsers(saveModelIPU.IdPU, generatorDescriptons.Generate(saveModelIPU), User.Identity.GetFIOFull());
                counter.UpdateReadings(saveModelIPU);
                return Content("");
            }catch(Exception ex)
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
                logger.ActionUsers(IdPU, "Удалил ПУ", User.Identity.GetFIOFull());
                counter.DeleteIPU(IdPU);
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
                counter.DeleteError(IdPU, Lic,User.Identity.GetFIOFull());
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
                var Result = readFileBank.Read(fileData, Bank);
                return View(Result);
            }
           
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UploadFilePU(HttpPostedFileBase file, string User,int TypeLoad)
        {
            if (TypeLoad == 1)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(new Excel().LoadExcelPU(workbook, User, cacheApp));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                    }
                }
            }
            if(TypeLoad == 2)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var workbook = new XLWorkbook(file.InputStream);
                    wb.Worksheets.Add(new Excel().LoadExcelPUProperty(workbook, User, cacheApp));
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
            return Content(cacheApp.GetValueProgress(Name));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Export(TypeFile typeFile)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (typeFile.Equals(TypeFile.Counters))
                {
                    wb.Worksheets.Add(new Excel().CreateExcelCounters());
                }
                if (typeFile.Equals(TypeFile.Lic))
                {
                    wb.Worksheets.Add(new Excel().CreateExcelLic(User.Identity.Name,cacheApp));
                }
                if (typeFile.Equals(TypeFile.General))
                {
                    wb.Worksheets.Add(new Excel().CreateExcelGeneral());
                }
                if (typeFile.Equals(TypeFile.ReestrIPU))
                {
                    wb.Worksheets.Add(new Excel().ReestrIPU(User.Identity.Name, cacheApp));
                }
                if (typeFile.Equals(TypeFile.EbdAll))
                {
                    return File(_ebd.CreateEBDAll(), "application/octet-stream","ebd.xml");
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile)+".xlsx");
                }
            }
            // 
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RunIntegration(DateTime date)
        {
            await _integration.LoadReadings("Integration", cacheApp,date);
            return null;
        }
        public ActionResult ErrorIntegration()
        {
            return View(_integration.GetErrorIntegrationReadings().OrderByDescending(x=>x.IsError));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ErroIntegratinLoadExcel()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(new Excel().ErroIntegratin());
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Интеграция .xlsx");
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ErroIntegratinDelete(string Lic,string TypePU)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Result = db.IntegrationReadings.Where(x => x.Lic == Lic && x.TypePu == TypePU && x.IsError == true).ToList();
                foreach(var Items in Result)
                {
                    Items.IsError = false;
                    db.SaveChanges();
                }
                return Redirect("/Counter/ErrorIntegration");
            }
        }

    }
}