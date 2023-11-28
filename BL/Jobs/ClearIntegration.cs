using BL.Loggers;
using BL.Notification;
using DB.DataBase;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenerator;

namespace BL.Jobs
{
    public class ClearIntegration : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using(var db = new ApplicationDbContext())
            {
                var date = DateTime.Now.AddMonths(-2);
                var res = db.IntegrationReadings.Where(x => x.DateTime <= date).ToList();
                ShedulerLogger.WhriteToFile($"Начало очистки интеграции {res.Count()}");
                foreach (var Item in res)
                {
                    db.IntegrationReadings.Remove(Item);
                    db.SaveChanges();
                }
               
            }
        }
    }
}
