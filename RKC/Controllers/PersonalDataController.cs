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
using DB.Model;
using DB.DataBase;
using DB.Extention;
using BL.http;
using System.Net.Http;
using BE.Counter;

namespace RKC.Controllers
{
    [Auth]
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
        private readonly IDictionary _dictionary;
        private readonly NLog.Logger _Nlogger = NLog.LogManager.GetCurrentClassLogger();
        public PersonalDataController(IPersonalData personalData, Ilogger logger, IGeneratorDescriptons generatorDescriptons,
            ICacheApp cacheApp, IFlagsAction flagsAction, ICounter counter, IBaseService baseService, IPdfFactory pdfFactory, ICourt court, IDictionary dictionary)
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
            _dictionary = dictionary;
        }
        [Auth(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult PersonalInformation(string FullLic)
        {
            ViewBag.FULL_LIC = FullLic;
            ViewBag.StateCalc = _personalData.GetStateCalculation(FullLic); 
            return View(_personalData.GetPersonalInformation(FullLic));
        }
        [Auth(Roles = "PersWriter,PersReader,Admin")]
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
                ViewBag.FlatTypeDic = _dictionary.GetFlatType();
                ViewBag.Benefit = _dictionary.GetAllBenefit();
                ViewBag.FlatType = _baseService.GetFlatTypeLic(FULL_LIC);
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
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult SaveFile(HttpPostedFileBase FileLoad, string NameFile,string Lic,int idPersData,string Fio)
        {
            return Json(new { result = _personalData.SaveFile(ConverToBytes(FileLoad), idPersData, Fio, Lic, FileLoad.FileName.Split('.').LastOrDefault(), NameFile, User.Identity.GetFIOFull()),
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
        [Auth(Roles = RolesEnums.ShowNoteLic)]
        [HttpGet]
        public ActionResult GetNoteAllLic(string FullLic)
        {
            var Result = _personalData.GetNoteAllLic(FullLic);
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
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.DownLoadReceipt)]
        [HttpGet]
        public ActionResult DownLoadReceipt(string FullLic, DateTime DateStart, DateTime DateEnd)
        {
            if(DateStart == DateEnd)
            {
                try
                {

                    var result = _pdfFactory.CreatePdf(PdfType.Personal).Generate(FullLic, DateEnd);
                    return File(result.FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.FileName);
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
                    try
                    {
                        zip.AddEntry(Items.FileName, Items.FileBytes);
                    }
                    catch
                    {

                    }
                }
                zip.Save(outputStream);
            }
            outputStream.Position = 0;
            return File(outputStream, "application/zip", "Квитанция.zip");
        }
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.DownLoadReceipt)]
        [HttpGet]
        public ActionResult ShwoReceipt(string FullLic, DateTime DateStart, DateTime DateEnd)
        {
            List<string> Content = new List<string>();
            if (DateStart == DateEnd)
            {
                try
                {

                    Content.Add(_pdfFactory.CreatePdf(PdfType.Personal).GenerateHtml(FullLic, DateEnd));
                }
                catch (Exception ex)
                {
                    return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
                }
            }
            else
            {
                while (DateStart >= DateEnd)
                {
                    try
                    {
                        Content.Add(_pdfFactory.CreatePdf(PdfType.Personal).GenerateHtml(FullLic, DateEnd));
                    }
                    catch (Exception ex)
                    {

                    }
                    DateEnd = DateEnd.AddMonths(1);
                }
            }
            return View(Content);
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + ","+ RolesEnums.SuperAdmin)]
        public ActionResult CloseLic(string FullLic)
        {
            _personalData.CloseLic(FullLic, _counter, User.Identity.GetFIO());
            return null;
        }
        [HttpGet]
        [Auth(Roles = RolesEnums.Admin + "," + RolesEnums.SuperAdmin)]
        public ActionResult OpenLic(string FullLic)
        {
            _personalData.OpenLic(FullLic);
            return null;
        }
        [HttpGet]
        [Auth(Roles = "PersWriter,Admin")]
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
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult SavePersonalData(PersDataModel persDataModel)
        {
            _Nlogger.Info(new ConvertJson<PersDataModel>(persDataModel).ConverModelToJson());
            _personalData.SavePersonalData(persDataModel, User.Identity.GetFIOFull());
            return null;
        }
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult AddPersData(PersDataModel persDataModel)
        {
            _personalData.AddPersData(persDataModel, User.Identity.GetFIOFull());
            return null;
        }
        [Auth(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult HistoryEdit(int idPersData)
        {
            return PartialView(_personalData.GetHistory(idPersData));
        }
        [HttpGet]
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult EditMain(int idPersData)
        {
            _personalData.MakeToMain(idPersData, User.Identity.GetFIOFull());
            return null;
        }
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult FormAddPers(string FULL_LIC)
        {
            ViewBag.RoomType = _personalData.GetRoomTypeMain(FULL_LIC);
            ViewBag.FULL_LIC = FULL_LIC;
            return PartialView();
        }
        [Auth(Roles = "PersWriter,Admin")]
        public ActionResult DeletePersonalData(int IdPersData)
        {
            _personalData.DeletePers(IdPersData,User.Identity.GetFIOFull());
            return null;
        }
        [Auth(Roles = "PersWriter,PersReader,Admin")]
        public ActionResult DetailedInformPersDelete(string FULL_LIC)
        {
            ViewBag.FULL_LIC = FULL_LIC;
            ViewBag.FlatTypeDic = _dictionary.GetFlatType();
            ViewBag.FlatType = _baseService.GetFlatTypeLic(FULL_LIC);
            return View(_personalData.GetInfoPersDataDelete(FULL_LIC));
        }
        public ActionResult PaymentHistoryView(string FullLic)
        {
            ViewBag.LIC = FullLic;
            return View(_personalData.GetPaymentHistory(FullLic).Where(x => x.transaction_amount != 0).OrderByDescending(x=>x.payment_date_day));
        }
        public ActionResult ReadingsHistoryView(string FullLic)
        {
            ViewBag.LIC = FullLic;
            return View(_personalData.GetReadingsHistory(FullLic).OrderByDescending(x => x.payment_date_day));
        }
        [Auth(Roles = RolesEnums.SuperAdmin)]
        public ActionResult ExaminationPersIsLic()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
               // var workbook = new XLWorkbook();
                wb.Worksheets.Add(new CheckToExcel().PersIsLic(User.Identity.Name,_cacheApp));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет проверки персов.xlsx");
                }
            }
        }
        public ActionResult NotSendReceipt()
        {
            using (var context = new ApplicationDbContext())
            {
                return View(context.NotSendReceipts.Filter().ToList());
            }    
        }
        public ActionResult DownloadNotSendReceiptExcels()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(NotificationExcel.CreateExcelReceiptNotSend());
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет по отправлению ЭПД_{DateTime.Now.AddMonths(-1).ToString("MM yyyy")}.xlsx");
                }
            }
        }
        [Auth(Roles = RolesEnums.SuperAdmin)]
        public HttpResponseMessage CleareNotSendReceiptExcels()
        {
            using (var context = new ApplicationDbContext())
            {
                context.NotSendReceipts.RemoveRange(context.NotSendReceipts.ToList());
                context.SaveChanges();
            }
            return Resposne.CreateResponse200();
        }
        [Auth(Roles = RolesEnums.SuperAdmin + "," + RolesEnums.ShowNoteLic)]
        public async Task<ActionResult> HistoryRecalculationView(string FullLic)
        {
            ViewBag.LIC = FullLic;
            var result = await _counter.GetRecalculations(FullLic);
            return View(result);
        }
    }
}