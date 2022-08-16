using BE.ApiT_;
using BL.ApiT_;
using BL.Counters;
using BL.Extention;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using WordGenerator;

namespace RKC.Controllers
{
    public enum Question
    {
        Role = 2,
        ProjectFunding = 3,
        TotalEmployee = 4,
        NumberOfServers = 5,
        TopBusinessConcern = 6
    }
    public class sss<T> where T : Enum
    {
        int result { get; set; }
        public T ss  { get; set; }
        public sss( T value)
        {
            ss = value;
        }
        public int Get()
        {
            
            var values = Enum.GetValues(typeof(T));
            foreach (int item in values)
                result = (int)item;
            return result;
        }
    }
    public class HomeController : Controller
    {
        IEBD _iEBD;
        public HomeController(IEBD iEBD)
        {
            _iEBD = iEBD;
            //_iEBD.CreateEBDAll();
            //Question t = new Question();
            //sss<Question> sss = new sss<Question>(t);
            //var s = sss.Get();
            //GenerateFileHelpCalculation.Generate("721077614", new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1));
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