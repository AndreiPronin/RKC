using AppCache;
using BL.Helper;
using BL.Service;
using BL.Services;
using RKC.Extensions;
using BE.PersData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WordGenerator;
using Ionic.Zip;
using ClosedXML.Excel;
using static System.Net.WebRequestMethods;
using BL.Excel;
using System.Threading.Tasks;
using BE.Roles;
using BL.Counters;
using WordGenerator.Enums;

namespace RKC.Controllers
{
    [Authorize]
    public class PersonalDataController : Controller
    {
        private readonly object balanceLock = new object();
        private readonly IPersonalData _personalData;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly ICacheApp _cacheApp;
        public readonly IFlagsAction _flagsAction;
        private readonly ICounter _counter;
        private readonly IBaseService _baseService;
        private readonly IPdfFactory _pdfFactory;
        private readonly ICourt _court;
        public PersonalDataController(IPersonalData personalData, Ilogger logger, IGeneratorDescriptons generatorDescriptons,
            ICacheApp cacheApp, IFlagsAction flagsAction, ICounter counter, IBaseService baseService, IPdfFactory pdfFactory, ICourt court)
        {
            _counter = counter;
            _personalData = personalData;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _baseService = baseService;
            _pdfFactory = pdfFactory;
            _court = court;
        }
        [Authorize(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult PersonalInformation(string FullLic)
        {
            ViewBag.FULL_LIC = FullLic;
            ViewBag.StateCalc = _personalData.GetStateCalculation(FullLic); 
            return View(_personalData.GetPersonalInformation(FullLic));
        }
        [Authorize(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult DetailedInformPersData(string FULL_LIC)
        {
            try
            {
                if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(DetailedInformPersData) + FULL_LIC))
                {
                    ViewBag.User = _cacheApp.GetValue(nameof(DetailedInformPersData) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                if (ViewBag.IsLock == false)
                {
                    ViewBag.IsLock = _flagsAction.GetAction(nameof(DetailedInformPersData));
                }
                ViewBag.ZAK = _baseService.GetStatusCloseOpenLic(FULL_LIC);
                var Result = _personalData.GetInfoPersData(FULL_LIC);
                if (Result.Count() > 0)
                {
                    return View(Result);
                }
                else
                {
                    if (Result.Count() == 0)
                    {
                        ViewBag.FULL_LIC = FULL_LIC;
                    }
                    return View(Result);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
            }
        }
        [HttpGet]
        public ActionResult clearCache(string Page)
        {
            _cacheApp.Delete(User.Identity.GetFIOFull(), Page);
            return null;
        }
        [HttpPost]
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult SaveFile(HttpPostedFileBase FileLoad, string NameFile,string Lic,int idPersData,string Fio)
        {
            return Json(new { result = _personalData.saveFile(ConverToBytes(FileLoad), idPersData, Fio, Lic, FileLoad.FileName.Split('.').LastOrDefault(), NameFile, User.Identity.GetFIOFull()),
                JsonRequestBehavior.AllowGet });
        }
        [HttpGet]
        public ActionResult DownLoadFile(int Id)
        {
            var Result = _personalData.DownLoadFile(Id);
            return File(Result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Result.FileName);
        }
        [HttpGet]
        public ActionResult ShowAllDocument(string FullLic)
        {
            var Result = _personalData.GetInfoPersData(FullLic);
            return PartialView(Result);
        }
        [HttpGet]
        public async Task<ActionResult> WatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            return View( await _personalData.GetInfoHelpСalculation(FullLic, DateFrom, DateTo));
        }
        [HttpGet]
        public async Task<ActionResult> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var Result = await _personalData.DownLoadHelpСalculation(FullLic, DateFrom, DateTo);
                _cacheApp.Delete(FullLic);
                return File(Result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Result.FileName);
            }catch(Exception ex)
            {
                _cacheApp.Update(FullLic, ex.InnerException.Message);
                return null;
            }
        }
        [Authorize(Roles = "Admin,DownLoadReceipt")]
        [HttpGet]
        public ActionResult DownLoadReceipt(string FullLic, DateTime DateStart, DateTime DateEnd)
        {
            if(DateStart == DateEnd)
            {
                //try
                //{

                var result = _pdfFactory.CreatePdf(PdfType.Personal).Generate(FullLic, DateEnd);
                return File(result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.FileName);
                //}catch(Exception ex)
                //{
                //    return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                //}
            }
            List<PersDataDocumentLoad> persData = new List<PersDataDocumentLoad>();
            while (DateStart >= DateEnd)
            {
                try
                {
                    persData.Add(_pdfFactory.CreatePdf(PdfType.Personal).Generate(FullLic, DateEnd));
                }
                catch (Exception ex)
                {
                   
                }
                DateEnd = DateEnd.AddMonths(1);
            }
            var outputStream = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                foreach (var Items in persData)
                {
                    zip.AddEntry(Items.FileName,Items.FileBytes);
                }
                zip.Save(outputStream);
            }
            outputStream.Position = 0;
            return File(outputStream, "application/zip", "Квитанция.zip");
        }
        [HttpGet]
        [Authorize(Roles = RolesEnums.Admin + ","+ RolesEnums.SuperAdmin)]
        public ActionResult CloseLic(string FullLic)
        {
            _personalData.CloseLic(FullLic, _counter);
            return null;
        }
        [HttpGet]
        [Authorize(Roles = RolesEnums.Admin + "," + RolesEnums.SuperAdmin)]
        public ActionResult OpenLic(string FullLic)
        {
            _personalData.OpenLic(FullLic);
            return null;
        }
        [HttpGet]
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult DeleteFile(int Id)
        {
            _personalData.DeleteFile(Id,User.Identity.GetFIOFull());
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
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult SavePersonalData(PersDataModel persDataModel)
        {
            _personalData.SavePersonalData(persDataModel, User.Identity.GetFIOFull());
            return null;
        }
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult AddPersData(PersDataModel persDataModel)
        {
            _personalData.AddPersData(persDataModel, User.Identity.GetFIOFull());
            return null;
        }
        [Authorize(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult HistoryEdit(int idPersData)
        {
            return PartialView(_personalData.GetHistory(idPersData));
        }
        [HttpGet]
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult EditMain(int idPersData)
        {
            _personalData.MakeToMain(idPersData, User.Identity.GetFIOFull());
            return null;
        }
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult FormAddPers(string FULL_LIC)
        {
            ViewBag.RoomType = _personalData.GetRoomTypeMain(FULL_LIC);
            ViewBag.FULL_LIC = FULL_LIC;
            return PartialView();
        }
        [Authorize(Roles = "PersWriter,Admin")]
        public ActionResult DeletePersonalData(int IdPersData)
        {
            _personalData.DeletePers(IdPersData,User.Identity.GetFIOFull());
            return null;
        }
        [Authorize(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult DetailedInformPersDelete(string FULL_LIC)
        {
            ViewBag.FULL_LIC = FULL_LIC;
            return View(_personalData.GetInfoPersDataDelete(FULL_LIC));
        }
        public ActionResult PaymentHistoryView(string FullLic)
        {
            ViewBag.LIC = FullLic;
            return View(_personalData.GetPaymentHistory(FullLic).OrderByDescending(x=>x.payment_date_day));
        }
        public ActionResult ReadingsHistoryView(string FullLic)
        {
            ViewBag.LIC = FullLic;
            return View(_personalData.GetReadingsHistory(FullLic).OrderByDescending(x => x.payment_date_day));
        }
        [Authorize(Roles = RolesEnums.SuperAdmin)]
        public ActionResult ExaminationPersIsLic()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workbook = new XLWorkbook();
                wb.Worksheets.Add(new ExaminationToExcel().ExaminationPersIsLic(User.Identity.Name,_cacheApp));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет проверки персов.xlsx");
                }
            }
        }
    }
}