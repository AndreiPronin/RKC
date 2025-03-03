using BE.Court;
using BE.MkdInformation;
using BE.Roles;
using BL.Excel;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    [Authorize(Roles = RolesEnums.MkdReader + "," + RolesEnums.Admin + "," + RolesEnums.SuperAdmin)]
    public class MKDController : Controller
    {
        private readonly IMkdInformationService _mkdInformationService;
        private readonly IExcelMkd _excelMkd;
        public MKDController(IMkdInformationService mkdInformationService, IExcelMkd excelMkd) 
        {
            _mkdInformationService = mkdInformationService;
            _excelMkd = excelMkd;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchMkd(BE.MkdInformation.SearchModel searchModel)
        {
            var result = _mkdInformationService.SearchMkd(searchModel);
            return PartialView(result);
        }
        public ActionResult MainInformation(int Id)
        {
            var result = _mkdInformationService.GetAddressMKD(Id);
            return View(result);
        }
        public ActionResult HistoryOdpu(int Id, DateTime DateFrom, DateTime DateTo)
        {
            var result = _mkdInformationService.GetHistoryOdpu(Id,DateFrom,DateTo);
            return View(result);
        }
        public ActionResult HistoryValueOdpu(int AddressId, string Address)
        {
            var result = _mkdInformationService.GetHistoryValueOdpu(AddressId);
            ViewBag.Address = Address;
            ViewBag.AddressId = AddressId;
            return View(result);
        }
        public ActionResult ListFlats(int Id, string Address)
        {
            var result = _mkdInformationService.GetListFlats(Id);
            ViewBag.Address = Address;
            ViewBag.AddressId = Id;
            return View(result);
        }
        public ActionResult ListFlatsExcel(int AddressId, string Address)
        {
            var result = _excelMkd.GetListFlats(AddressId);
            return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, $"Список помещений {Address}.xlsx");
        }
        public ActionResult HistoryRecalculationView(int AddressId, string Address)
        {
            ViewBag.Address = Address;
            var result = _mkdInformationService.HistoryRecalculation(AddressId);
            return View(result);
        }
    }
}