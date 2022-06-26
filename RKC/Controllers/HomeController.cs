using BL.Counters;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        { 
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ResultEmpty(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }
    }
}