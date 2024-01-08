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
using BL.Security;
using BL.http;
using BE.Counter;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace RKC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEBD _eBD;
        private readonly ICacheApp _cacheApp;
        private readonly ITokenCreator _tokenCreator;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public HomeController(IEBD eBD, ICacheApp cacheApp, ITokenCreator tokenCreator)
        {
            _eBD = eBD;
            _cacheApp = cacheApp;
            _tokenCreator = tokenCreator;
        }
        public async Task<ActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }
        public async Task<ActionResult> GetFile()
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var url = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceUrl).GetString();
            var reult = await Reuqests.GetRequestWithTockenAsync($"{url}/api/v1/TextReports/GetSberbankSevens?period=2023-10-31", token);

            return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
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