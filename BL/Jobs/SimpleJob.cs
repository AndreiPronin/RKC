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
    public class SimpleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            using(var db = new ApplicationDbContext())
            {
                var date = DateTime.Now.AddMonths(-2);
                var res = await db.IntegrationReadings.Where(x => x.DateTime <= date).ToListAsync();
                foreach(var Item in res)
                {
                    db.IntegrationReadings.Remove(Item);
                    await db.SaveChangesAsync();
                }
               
            }
            await Task.CompletedTask;
        }
    }
}
