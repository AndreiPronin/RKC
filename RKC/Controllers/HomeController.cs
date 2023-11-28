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
using BL.Helper;
using Microsoft.AspNet.Identity;
using RKC.Extensions;
using System.Security.Principal;
using System.Net.Http;
using System.Web.Http.Results;
using NLog;

namespace RKC.Controllers
{

    public class HomeController : Controller
    {
        private readonly IEBD _eBD;
        private readonly ICacheApp _cacheApp;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public HomeController(IEBD eBD, ICacheApp cacheApp)
        {
            _eBD = eBD;
            _cacheApp = cacheApp;
            //var rrr = Parser.PdfParser();
        }
        public ActionResult Index()
        {
            var sss = User.Identity.IsAuthenticated;
           
            //_eBD.CreateEbdFlatliving(DateTime.Now);
            var res = new GetConfigurationManager().GetAppSettings("").GetInt();
            return View();
        }
        [Auth]
        public ActionResult IndexUnLock()
        {
            if (User != null)
            {
                var userName = User.Identity.Name;
                if (!string.IsNullOrEmpty(userName))
                    _cacheApp.Delete(userName);
            }
            return Redirect("Index");
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
        public ActionResult AccessDenied()
        {
            return View();
        }
        public bool CheckNewRole(string UserName)
        {
            var user = _cacheApp.GetValue(UserName);
            if (!string.IsNullOrEmpty(user))
            {
                return true;
            }
            return false;
        }
    }
}