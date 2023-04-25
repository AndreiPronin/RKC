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

namespace RKC.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ISecurityProvider _securityProvider;
        private readonly IFlagsAction _flagsAction;
        public AdminController(ISecurityProvider securityProvider, IFlagsAction flagsAction)
        {
            _securityProvider = securityProvider;
            _flagsAction = flagsAction;
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
            }catch(Exception ex)
            {
                throw ex;
            }
            return Content("Роль успешно удалена");
        }
        [Auth(Roles = "Admin")]
        public ActionResult AddRole(string UserId, string UserRoleId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.AspNetUserRoles.Add(new DB.Model.AspNetUserRoles { UserId = UserId, RoleId = UserRoleId });
                db.SaveChanges();
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
                var ErrorIntegration = db.IntegrationReadings.Where(x => x.IsError == true).Select(x => x.IsError).Count();
                if (ErrorIntegration > 0)
                {
                    notification.Add(new Notifications { Description = $"Ошибка показаний {ErrorIntegration} ошибки", Title = "Ошибка показаний ИПУ" });
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
    }
}