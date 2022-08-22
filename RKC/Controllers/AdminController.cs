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
                var ErrorIntegration = db.IntegrationReadings.Select(x => x.IsError).Count();
                notification.Add(new Notifications { Description = $"Ошибка интеграции {ErrorIntegration} ошибки", Title="ErrorIntegration" });
                return PartialView(notification);
            }
        }

    }
}