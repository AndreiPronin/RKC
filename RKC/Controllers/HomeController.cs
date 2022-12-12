using AppCache;
using BL;
using BL.ApiT_;
using DB.DataBase;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RKC.Controllers
{

    public class HomeController : Controller
    {
       
        public HomeController(IEBD eBD)
        {
            using (var db = new DbLIC())
            {

                //WordGenerator.GenerateFileHelpCalculation.Generate("703027614", new DateTime(2022, 10, 01));
         
            }
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