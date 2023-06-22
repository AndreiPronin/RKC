using BE.Court;
using BE.MkdInformation;
using BE.Roles;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Controllers
{
    [Authorize(Roles = RolesEnums.MkdReader + "," + RolesEnums.Admin + "," + RolesEnums.SuperAdmin)]
    public class MKDController : Controller
    {
        private readonly IMkdInformationService _mkdInformationService;
        public MKDController(IMkdInformationService mkdInformationService) {
            _mkdInformationService = mkdInformationService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchMkd(BE.MkdInformation.SearchModel searchModel)
        {
            var result = _mkdInformationService.SearchMkd(searchModel);
            return PartialView(result);
        }
        public ActionResult MainInformation(int Id)
        {
            var result = _mkdInformationService.GetAddressMKD(Id);
            return View(result);
        }
    }
}