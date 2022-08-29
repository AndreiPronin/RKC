using BE.Admin;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Security
{
    public interface ISecurityProvider
    {
        List<AspNetRoles> GetAllRoles();
        List<AspNetUsers> GetAllUser();
        List<AspNetUserRoles> GetAllUserRoles();
        List<UserRoleInfo> GetUserRoles(string Name);
        bool GetRoleUserNoLock(string UserId);
    }
    public class SecurityProvider : ISecurityProvider
    {
        public List<AspNetRoles> GetAllRoles()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.AspNetRoles.ToList();
            }
        }

        public List<AspNetUsers> GetAllUser()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.AspNetUsers.ToList();
            }
        }

        public List<AspNetUserRoles> GetAllUserRoles()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.AspNetUserRoles.ToList();
            }
        }

        public List<UserRoleInfo> GetUserRoles(string UserId)
        {
            using(var db = new ApplicationDbContext())
            {
                List<UserRoleInfo> userRoleInfo = new List<UserRoleInfo>();
                var User = db.AspNetUserRoles.Where(x=>x.UserId == UserId).ToList();
                var UserFio = db.AspNetUsers.FirstOrDefault(x => x.Id == UserId).FIO;
                foreach (var Items in User)
                {
                    var Role = db.AspNetRoles.FirstOrDefault(x => x.Id == Items.RoleId);
                    userRoleInfo.Add(new UserRoleInfo { UserFio = UserFio, UserRole = Role.Description, UserId = Items.UserId, UserRoleId = Items.RoleId });
                }
                return userRoleInfo;
            }
        }
        public bool GetRoleUserNoLock(string UserId)
        {
            using (var db = new ApplicationDbContext())
            {
                var UserRole = db.AspNetUserRoles.Where(x => x.UserId == UserId && x.RoleId == "8").FirstOrDefault();
                if (UserRole == null) return true;
                else return false;
            }
        }
    }
}
