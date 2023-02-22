using AppCache;
using BL;
using BL.ApiT_;
using DB.DataBase;
using DB.Query;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Owin;
using System;
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