using AppCache;
using BL.Helper;
using BL.Security;
using BL.Services;
using Microsoft.AspNet.Identity;
using RKC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    [Authorize(Roles = "Admin,Сourt")]
    public class СourtController : Controller
    {
        private readonly ICourt _Сourt;
        private readonly Ilogger _logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly ICacheApp _cacheApp;
        public readonly IFlagsAction _flagsAction;
        public readonly ISecurityProvider _securityProvider;
        public СourtController(ICourt counter, Ilogger logger, IGeneratorDescriptons generatorDescriptons,
            ICacheApp cacheApp, IFlagsAction flagsAction,
            ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
            _logger = logger;
            _generatorDescriptons = generatorDescriptons;
            _cacheApp = cacheApp;
            _flagsAction = flagsAction;

        }
        // GET: Сourt
        public ActionResult DetailedСourtData(string FULL_LIC)
        {
            try
            {
                if (_cacheApp.Lock(User.Identity.GetFIOFull(), nameof(DetailedСourtData) + FULL_LIC))
                {
                    ViewBag.User = _cacheApp.GetValue(nameof(DetailedСourtData) + FULL_LIC);
                    ViewBag.IsLock = true;
                }
                else ViewBag.IsLock = false;
                var Result = _Сourt.DetailInfroms(FULL_LIC);
                if (ViewBag.IsLock == true) ViewBag.IsLock = _securityProvider.GetRoleUserNoLock(User.Identity.GetUserId());
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
    }
}