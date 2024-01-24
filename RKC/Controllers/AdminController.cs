using BE.Service;
using BL.Security;
using DB.DataBase;
using DB.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RKC.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationDbContext = DB.DataBase.ApplicationDbContext;
using ApplicationContext = RKC.Models.ApplicationDbContext;
using Microsoft.AspNet.Identity.Owin;
using RKC.Extensions;
using BL.Helper;
using System.Data.Entity;
using System.Threading.Tasks;
using BE.Roles;
using AppCache;
using BE.Counter;
using DB.Extention;
using BL.Services;
using BE.Admin.Enums;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using System.IO;

namespace RKC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISecurityProvider _securityProvider;
        private readonly IFlagsAction _flagsAction;
        private readonly ICacheApp _cacheApp;
        private readonly IApiReportService _apiReportService;
        public AdminController(ISecurityProvider securityProvider, IFlagsAction flagsAction, ICacheApp cacheApp, IApiReportService apiReportService)
        {
            _securityProvider = securityProvider;
            _flagsAction = flagsAction;
            _cacheApp = cacheApp;
            _apiReportService = apiReportService;
        }
        [Auth(Roles = "Admin")]
        public ActionResult Index()
        {
            if(User.IsInRole("SuperAdmin"))
                ViewBag.Roles = _securityProvider.GetAllRoles();
            if (User.IsInRole("Admin") && !User.IsInRole("SuperAdmin"))
                ViewBag.Roles = _securityProvider.GetAllRolesWhithoutBoosRoles();
            ViewBag.User = _securityProvider.GetAllUser();

            ViewBag.StatusDetailedInformIPU = _flagsAction.GetAction("DetailedInformIPU");
            ViewBag.DetailedInformPersData = _flagsAction.GetAction("DetailedInformPersData");
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Notifacation = db.Notifications.Where(x => x.IsDelete == false).ToList();
                ViewBag.IntegrationTime = db.Flags.Find(((int)EnumFlags.LastIntegration)).DateTime;
                ViewBag.LastLoadEbd = db.Flags.Find(((int)EnumFlags.LastLoadEbd)).DateTime;
            }
                
            return View();
        }
        public async Task<JsonResult> GetEmployee(string FullName)
        {
            using (var db = new ApplicationDbContext())
            {
                var result  =  await db.AspNetUsers.Where(x => x.FIO.StartsWith(FullName)).ToListAsync();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [Auth(Roles = "Admin")]
        public ActionResult GetUserRoleInfo(string UserId)
        {
            return PartialView(_securityProvider.GetUserRoles(UserId));
        }
        [Auth(Roles = "Admin")]
        public ActionResult DeleteRole(string UserId, string UserRoleId)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                    var roleName = new ApplicationDbContext().AspNetRoles.FirstOrDefault(x => x.Id == UserRoleId);
                    userManager.RemoveFromRoles(UserId, roleName.Name);
                   
                }
                using (var db = new ApplicationDbContext())
                {
                    var LockUser = db.AspNetUsers.Find(UserId);
                    _cacheApp.AddInfinity(LockUser.UserName, UserRoleId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Content("Роль успешно удалена");
        }
        [Auth(Roles = RolesEnums.SuperAdmin)]
        public ActionResult DeleteUser(string UserId)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                    var user = userManager.FindById(UserId);
                    userManager.Delete(user);
                }
                using (var db = new ApplicationDbContext())
                {
                    var LockUser = db.AspNetUsers.Find(UserId);
                    _cacheApp.AddInfinity(LockUser.UserName, UserId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content("Пользователь успешно удален!");
        }
        [Auth(Roles = "Admin")]
        public ActionResult AddRole(string UserId, string UserRoleId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.AspNetUserRoles.Add(new DB.Model.AspNetUserRoles { UserId = UserId, RoleId = UserRoleId });
                db.SaveChanges();
                var LockUser =  db.AspNetUsers.Find(UserId);
               _cacheApp.AddInfinity(LockUser.UserName, UserRoleId);
            }
            return Content("Роль успешно добавлена");
        }
        [Auth(Roles = "Admin")]
        public ActionResult AddNotifications(string Title, string Description)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Notifications.Add( new Notifications { Description = Description, Title = Title });
                db.SaveChanges();
            }
            return Content("Уведомление успешно добавлено");
        }
        [Auth(Roles = "Admin,Notifications")]
        public ActionResult GetNotification()
        {
            using (var db = new ApplicationDbContext()) 
            {
                var notification = db.Notifications.Where(x => x.IsDelete == false).ToList();
                var ErrorIntegration = db.IntegrationReadings.Filter().Select(x => x.IsError)?.Count();
                var notSendElectonicalReceipt = db.NotSendReceipts.Filter().Select(x=>x.IsSend).Count();
                if (ErrorIntegration > 0)
                {
                    notification.Add(new Notifications { Description = $"Ошибка показаний {ErrorIntegration} ошибки", Title = "Ошибка показаний ИПУ" });
                }
                if (notSendElectonicalReceipt > 0)
                {
                    notification.Add(new Notifications { Description = $"Ошибка отправки почты {notSendElectonicalReceipt} ошибки", Title = "Ошибка отправки почты" });
                }
                return PartialView(notification);
            }
        }
        [Auth(Roles = "Admin")]
        public ActionResult deleteNotification(int Id)
        {
            using(var db = new ApplicationDbContext())
            {
                db.Notifications.Remove(db.Notifications.Find(Id));
                db.SaveChanges();
            }
            return Redirect("/Admin");
        }
        [HttpPost]
        [Auth]
        public async Task<ActionResult> DownloadReport(ApiReportEnums TypeReport, DateTime? ReportDate, HttpPostedFileBase file)
        {
            try
            {
                byte[] result;
                switch (TypeReport)
                {
                    case ApiReportEnums.GetSberbankInvoicesOldFormat:
                        result = await _apiReportService.GetSberbankInvoicesOldFormat(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/zip", ApiReportEnums.GetSberbankInvoicesOldFormat.GetDescription());
                    case ApiReportEnums.GetSberbankInvoices:
                        result = await _apiReportService.GetSberbankInvoices(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/zip", ApiReportEnums.GetSberbankInvoices.GetDescription());
                    case ApiReportEnums.GetSberbankCounters:
                        result = await _apiReportService.GetSberbankCounters(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/zip", ApiReportEnums.GetSberbankCounters.GetDescription());
                    case ApiReportEnums.GetRecalculation:
                        result = await _apiReportService.GetRecalculation();
                        return File(result, "application/octet-stream", ApiReportEnums.GetRecalculation.GetDescription());
                    case ApiReportEnums.GetNss:
                        result = await _apiReportService.GetNss(ReportDate.HasValue ? ReportDate.Value : DateTime.Now, file.InputStream, file.FileName);
                        return File(result, "application/zip", ApiReportEnums.GetNss.GetDescription());
                    case ApiReportEnums.GetSubagent:
                        result = await _apiReportService.GetSubagent(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/octet-stream", ApiReportEnums.GetSubagent.GetDescription());
                    case ApiReportEnums.GetShortSaldo:
                        result = await _apiReportService.GetShortSaldo(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/octet-stream", ApiReportEnums.GetShortSaldo.GetDescription());
                    case ApiReportEnums.GetFullSaldo:
                        result = await _apiReportService.GetFullSaldo(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/octet-stream", ApiReportEnums.GetFullSaldo.GetDescription());
                    case ApiReportEnums.GetInvoices:
                        result = await _apiReportService.GetInvoices(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/octet-stream", ApiReportEnums.GetInvoices.GetDescription());
                    case ApiReportEnums.GetConsumerData:
                        result = await _apiReportService.GetConsumerData(ReportDate.HasValue ? ReportDate.Value : DateTime.Now);
                        return File(result, "application/octet-stream", ApiReportEnums.GetConsumerData.GetDescription());
                    case ApiReportEnums.GetNssErrors:
                        result = await _apiReportService.GetNssErrors(ReportDate.HasValue ? ReportDate.Value : DateTime.Now, file.InputStream, file.FileName);
                        return File(result, "application/octet-stream", ApiReportEnums.GetNssErrors.GetDescription());

                    default:
                        return File(new byte[0], "Ошибка отчет не найде.txt");
                }
            }catch(Exception ex)
            {
                return Redirect("/Home/ResultEmpty?Message=" + ex.Message);
            }
        }
    }
}