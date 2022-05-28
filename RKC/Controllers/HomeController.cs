using BL.Counters;
using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICounter counter;
        public HomeController(ICounter _counter)
        {
            counter = _counter;
        }
        public ActionResult Index()
        {
            var FIO = User;
            return View();
        }
        public ActionResult ResultEmpty(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }
    }
}