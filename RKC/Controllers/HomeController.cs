using AppCache;
using AutoMapper;
using BE.Counter;
using BL.ApiT_;
using BL.Helper;
using BL.http;
using BL.Security;
using NLog;
using RKC.Extensions;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEBD _eBD;
        private readonly ICacheApp _cacheApp;
        private readonly ITokenCreator _tokenCreator;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        public HomeController(IEBD eBD, ICacheApp cacheApp, ITokenCreator tokenCreator , IMapper mapper)
        {
            _eBD = eBD;
            _cacheApp = cacheApp;
            _tokenCreator = tokenCreator;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            await Task.CompletedTask;
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