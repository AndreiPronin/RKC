using AppCache;
using BE.Admin;
using BE.DPU;
using BL;
using BL.ApiT_;
using BL.Services;
using DB.DataBase;
using DB.Model;
using DB.Query;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WordGenerator;
using WordGenerator.Enums;

namespace RKC.Controllers
{

    public class HomeController : Controller
    {
        private readonly IEBD _eBD;
        public HomeController(IEBD eBD)
        {
            _eBD = eBD;
            //var rrr = Parser.PdfParser();
        }
        public ActionResult Index()
        {
            var sss = User.Identity.IsAuthenticated;
            //_eBD.CreateEbdFlatliving(DateTime.Now);
            return View();
        }
        public ActionResult ResultEmpty(string Message)
        {
            
            ViewBag.Message = Message;
            return View();
        }

        public ActionResult Test()
        {
            var sss = User.Identity.IsAuthenticated;
            var tt2 = User.IsInRole("Admin");
            var tt = User;
            return Content("Test");
        }
    }
}