using AppCache;
using BL;
using BL.ApiT_;
using DB.DataBase;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
        IEBD _eBD;
        public HomeController(IEBD eBD)
        {
            _eBD = eBD;
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