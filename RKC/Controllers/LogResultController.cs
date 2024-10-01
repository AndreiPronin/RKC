using AppCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class LogResultController : Controller
    {
        private readonly ICacheApp _cacheApp;
        public LogResultController(ICacheApp cacheApp)
        {
            _cacheApp = cacheApp;
        }
        // GET: LogResult
        public ActionResult Index() => View();
        public ActionResult ShowLogResult(string Objects)
        {
            ViewBag.Objects = Objects;
            return PartialView();
        }
        public ActionResult GetLog(string Name)
        {
            var result = _cacheApp.GetValueProgress(Name);
            if(result == null)
                return Content("Загрузка.......");
            return Content(_cacheApp.GetValueProgress(Name));
        }
    }
}