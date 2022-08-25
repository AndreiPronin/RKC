using BL.Security;
using DB.DataBase;
using DB.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RKC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationDbContext = DB.DataBase.ApplicationDbContext;

namespace RKC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ISecurityProvider _securityProvider;
        public AdminController(ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
        }
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Roles = _securityProvider.GetAllRoles();
            ViewBag.User = _securityProvider.GetAllUser();
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Notifacation = db.Notifications.Where(x => x.IsDelete == false).ToList();
            }
                
            return View();
        }
        public ActionResult GetUserRoleInfo(string UserId)
        {
            return PartialView(_securityProvider.GetUserRoles(UserId));
        }
        public ActionResult DeleteRole(string UserId, string UserRoleId)
        {
            using(var db = new ApplicationDbContext())
            {
                var UserRole = db.AspNetUserRoles.FirstOrDefault(x => x.RoleId == UserRoleId && x.UserId == UserId);
                db.AspNetUserRoles.Remove(UserRole);
                db.SaveChanges();
            }
            return Content("Роль успешно удалена");
        }
        public ActionResult AddRole(string UserId, string UserRoleId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.AspNetUserRoles.Add(new DB.Model.AspNetUserRoles { UserId = UserId, RoleId = UserRoleId });
                db.SaveChanges();
            }
            return Content("Роль успешно добавлена");
        }
        public ActionResult AddNotifications(string Title, string Description)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Notifications.Add( new Notifications { Description = Description, Title = Title });
                db.SaveChanges();
            }
            return Content("Уведомление успешно добавлено");
        }
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