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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WordGenerator;
using WordGenerator.Enums;

namespace RKC.Controllers
{

    public class HomeController : Controller
    {
        public HomeController(IEBD eBD)
        {
            //var rrr = Parser.PdfParser();
        }
        public ActionResult Index()
        {
            //_eBD.CreateEBDAll();
            return View();
        }
        public ActionResult ResultEmpty(string Message)
        {
            
            ViewBag.Message = Message;
            return View();
        }
        
    }
}