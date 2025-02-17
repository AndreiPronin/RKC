﻿using BE.Court;
using BE.MkdInformation;
using BE.Roles;
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
        public MKDController(IMkdInformationService mkdInformationService) {
            _mkdInformationService = mkdInformationService;
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
        public ActionResult ListFlats(int Id, string Address)
        {
            var result = _mkdInformationService.GetListFlats(Id);
            ViewBag.Address = Address;
            return View(result);
        }
        public ActionResult HistoryRecalculationView(int AddressId, string Address)
        {
            ViewBag.Address = Address;
            var result = _mkdInformationService.HistoryRecalculation(AddressId);
            return View(result);
        }
    }
}