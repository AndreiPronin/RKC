using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    [Authorize]
    public class SocialHiringController : Controller
    {
        // GET: SocialHiring
        public ActionResult Index()
        {
            return View();
        }
    }
}