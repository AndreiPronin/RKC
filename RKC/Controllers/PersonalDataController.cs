using AppCache;
using BL.Helper;
using BL.Service;
using BL.Services;
using RKC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class PersonalDataController : Controller
    {
        private readonly IPersonalData _personalData;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly ICacheApp _cacheApp;
        public readonly IFlagsAction _flagsAction;
        public readonly IReadFileBank _readFileBank;
        public PersonalDataController(IPersonalData personalData, Ilogger logger, IGeneratorDescriptons generatorDescriptons,
            ICacheApp cacheApp, IFlagsAction flagsAction, IReadFileBank readFileBank)
        {
            _personalData = personalData;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;
            _readFileBank = readFileBank;
        }
        // GET: PersonalData
        public ActionResult DetailedInformPersData(string FULL_LIC)
        {
            try
            {
                if (_cacheApp.Lock(User.Identity.GetFIO(), nameof(DetailedInformPersData) + FULL_LIC))
                {
                    ViewBag.User = _cacheApp.GetValue(nameof(DetailedInformPersData) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                ViewBag.IsLock = _flagsAction.GetAction(nameof(DetailedInformPersData));
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
            _cacheApp.Delete(User.Identity.GetFIO(), Page);
            return null;
        }
    }
}