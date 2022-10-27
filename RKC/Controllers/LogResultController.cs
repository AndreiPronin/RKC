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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowLogResult()
        {
            return PartialView();
        }
        public ActionResult GetLog(string Name)
        {
            var ttt = _cacheApp.GetValueProgress(Name);
            return Content(_cacheApp.GetValueProgress(Name));
        }
    }
}