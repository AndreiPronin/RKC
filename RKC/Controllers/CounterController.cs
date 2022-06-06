using BE.Counter;
using BL.Counters;
using System;
using System.Collections.Generic;
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

namespace RKC.Controllers
{
    [Authorize]
    public class CounterController : Controller
    {
        private readonly ICounter counter;
        private readonly Ilogger logger;
        private readonly IGeneratorDescriptons generatorDescriptons;
        private readonly ICacheApp cacheApp;
        public readonly IFlagsAction flagsAction;
        public readonly IReadFileBank readFileBank;
        public CounterController(ICounter _counter, Ilogger _logger, IGeneratorDescriptons _generatorDescriptons, 
            ICacheApp _cacheApp, IFlagsAction _flagsAction, IReadFileBank _readFileBank)
        {
            counter = _counter;
            logger = _logger;
            generatorDescriptons = _generatorDescriptons;
            cacheApp = _cacheApp;
            flagsAction = _flagsAction;
            readFileBank = _readFileBank;
        }
        // GET: Counter
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
        public ActionResult DetailedInformIPU(string FULL_LIC)
        {
            try
            {
                if (cacheApp.Lock(User.Identity.GetFIO(), nameof(DetailedInformIPU) + FULL_LIC))
                {
                    ViewBag.User = cacheApp.GetValue(nameof(DetailedInformIPU) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                ViewBag.IsLock = flagsAction.GetAction(nameof(DetailedInformIPU));
                var Result = counter.DetailInfroms(FULL_LIC);
                
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
        public ActionResult FromAddPU(string FullLIC) 
        {
            ViewBag.FULL_LIC = FullLIC;
            return PartialView(new ModelAddPU());
        }
        [HttpPost]
        public ActionResult AddPU(ModelAddPU modelAddPU)
        {
            try
            {
                counter.AddPU(modelAddPU,User.Identity.GetFIO());
                return null;
            }catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult clearCache(string Page)
        {
            cacheApp.Delete(User.Identity.GetFIO(),Page);
            return null;
        }
        [HttpGet]
        public ActionResult ArchiveAllLics(string id_pu)
        {
            ViewBag.FULL_LIC = id_pu.Split('-')[0];
            ViewBag.TypePU = id_pu.Split('-')[1];
            var Result = counter.HistoryIndinikation(id_pu.Split('-')[0]);
            return PartialView(Result);
        }
        [HttpGet]
        public ActionResult HistoryEdit(int id_pu)
        {
            return PartialView(counter.HistoryEdit(id_pu));
        }
        [HttpPost]
        public ActionResult SaveIPU(SaveModelIPU saveModelIPU)
        {
            try
            {
                logger.ActionUsers(saveModelIPU.IdPU, generatorDescriptons.Generate(saveModelIPU), User.Identity.GetFIO());
                counter.UpdateReadings(saveModelIPU);
                return Content("");
            }catch(Exception ex)
            {
                return Content($"Во время обновления произошла ошибка {ex.Message}");
            }
        }
        [HttpPost]
        public ActionResult DeletePU(int IdPU)
        {
            try
            {
                logger.ActionUsers(IdPU, "Удалил ПУ", User.Identity.GetFIO());
                counter.DeleteIPU(IdPU);
                return Content("Удаление прошло успешно");
            }
            catch (Exception ex)
            {
                return Content($"Во время удаления произошла ошибка {ex.Message}");
            }
        }
        public ActionResult UploadFile(HttpPostedFileBase file, Banks Bank)
        {
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                byte[] fileData = binaryReader.ReadBytes(file.ContentLength);
                var Result = readFileBank.Read(fileData, Bank);
                return View(Result);
            }
           
        }
        public ActionResult UploadFilePU(HttpPostedFileBase file, string User)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workbook = new XLWorkbook(file.InputStream);
                wb.Worksheets.Add(new ExcelReader().LoadExcelPU(workbook,User));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ошибки.xlsx");
                }
            }
        }
        [HttpGet]
        public ActionResult Export(TypeFile typeFile)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (typeFile.Equals(TypeFile.Counters))
                {
                    wb.Worksheets.Add(new ExcelReader().CreateExcelCounters());
                }
                if (typeFile.Equals(TypeFile.Lic))
                {
                    wb.Worksheets.Add(new ExcelReader().CreateExcelLic());
                }
                if (typeFile.Equals(TypeFile.General))
                {
                    wb.Worksheets.Add(new ExcelReader().CreateExcelGeneral());
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", GetDescriptionEnum.GetDescription(typeFile)+".xlsx");
                }
            }
            // 
        }
       

    }
}