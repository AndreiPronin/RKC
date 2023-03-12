using AppCache;
using BE.Admin;
using BE.DPU;
using BL;
using BL.ApiT_;
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
        public HomeController()
        {
            using (var db = new ApplicationDbContext())
            {
                var ttt = db.CourtGeneralInformation.Include(x => x.CourtBankruptcy)
                    .Include(x => x.CourtInstallmentPlan)
                    .Include(x=>x.CourtExecutionInPF)
                    .Include(x => x.CourtLitigationWork)
                    .Include(x=>x.CourtWork)
                    .Include(x => x.CourtWriteOff)
                    .Include(x => x.CourtStateDuty)
                    .Include(c=>c.CourtDocumentScans).FirstOrDefault();
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