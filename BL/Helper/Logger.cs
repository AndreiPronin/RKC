using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public interface Ilogger
    {
        void ActionUsers(int IdPU, string Description, string User);
        void ActionUsersPersData(int idPersData, string Description, string User);
        Task ActionUsersAsync(int IdPU, string Description, string User);
    }
    public class Logger:Ilogger
    {
        public void ActionUsers(int IdPU, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Log.Add(new Log { IdPU = IdPU, Description = Description, UserName = User, DateTime = DateTime.Now });
                    db.SaveChanges();
                }
            }
        }
        public async Task ActionUsersAsync(int IdPU, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Log.Add(new Log { IdPU = IdPU, Description = Description, UserName = User, DateTime = DateTime.Now });
                    await db.SaveChangesAsync();
                }
            }
        }
        public void ActionUsersPersData(int idPersData, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.LogsPersData.Add(new LogsPersData { idPersData = idPersData, Description = Description, UserName = User, DateTime = DateTime.Now });
                    db.SaveChanges();
                }
            }
        }
    }
}
