using AppCache;
using BE.Court;
using BE.Roles;
using BL.Helper;
using BL.Security;
using BL.Services;
using Microsoft.AspNet.Identity;
using RKC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace RKC.Controllers
{
    [Authorize(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWhriter)]
    public class CourtController : Controller
    {
        private readonly ICourt _court;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly IDictionary _dictionary;
        private readonly ICacheApp _cacheApp;
        public readonly IFlagsAction _flagsAction;
        public readonly ISecurityProvider _securityProvider;
        public CourtController(ICourt court, Ilogger logger, IGeneratorDescriptons generatorDescriptons,IDictionary dictionary,
            ICacheApp cacheApp, IFlagsAction flagsAction,
            ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _dictionary = dictionary;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _court = court;
        }
        public async Task<ActionResult> Index(int Id = 0)
        {
            var Model = await _court.DetailInfroms(Id); 
            return View(Model);
        }
        public ActionResult Serach()
        {
            return View();
        }
        public async Task<ActionResult> SearchResult(SearchModel searchModel)
        {
            var Result =  await _court.Serach(searchModel);
            return PartialView(Result);
        }
        [Authorize(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> CreateCourt(string FullLic, string NumberIP)
        {
            if(string.IsNullOrEmpty(FullLic) || FullLic == "undefined")
                return Redirect("/Home/ResultEmpty?Message=" + "Нельзя указать пустой лицевой счет!");
            if (string.IsNullOrEmpty(NumberIP) || NumberIP == "undefined")
                return Redirect("/Home/ResultEmpty?Message=" + "Нельзя указать пустой номер судебного приказа!");
            var Result = await _court.CreateCourt(FullLic, NumberIP);
            return Redirect("/Court/Index?Id=" + Result);
        }
        [Authorize(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin)]
        public async Task<ActionResult> SaveCourt(CourtGeneralInformation courtGeneralInformation)
        {
            var Id =  await _court.SaveCourt(courtGeneralInformation);
            return Redirect("/Court/Index?Id=" + Id);
        }
        [Authorize(Roles = RolesEnums.CounterWriter + "," + RolesEnums.CourtAdmin + "," + RolesEnums.SuperAdmin + "," + RolesEnums.CourtWhriter)]
        public async Task<ActionResult> ShowAllCourtModal(string FullLic)
        {
            ViewBag.FullLic = FullLic;
            var Result = await _court.GetAllCourtFullLic(FullLic);
            return PartialView(Result);
        }
        public async Task<JsonResult> AutocompleteDictionary(string Text, int Id)
        {
            var result = await _dictionary.GetCourtNameDictionaries(Text,Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DetailedСourtData(string FULL_LIC)
        //{
        //    try
        //    {
        //        if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(DetailedСourtData) + FULL_LIC))
        //        {
        //            ViewBag.User = _cacheApp.GetValue(nameof(DetailedСourtData) + FULL_LIC);
        //            ViewBag.IsLock = true;
        //        }
        //        else ViewBag.IsLock = false;
        //        //var Result = _court.DetailInfroms(FULL_LIC);
        //        if (ViewBag.IsLock == true) ViewBag.IsLock = _securityProvider.GetRoleUserNoLock(User.Identity.GetUserId());
        //        //if (Result.Count() > 0)
        //        //{
        //        return View();
        //        //}
        //        //else
        //        //{
        //        //    if (Result.Count() == 0)
        //        //    {
        //        //        ViewBag.FULL_LIC = FULL_LIC;
        //        //    }
        //        //    return View(Result);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
        //    }
        //}
    }
}

