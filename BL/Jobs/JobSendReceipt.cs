using BE.JobManager;
using BL.Notification;
using DB.DataBase;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenerator;

namespace BL.Jobs
{
    internal class JobSendReceipt : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            IJobManager _jobmanager = new JobManager(new NotificationMail(), new ReceiptFactory());
            using(var dbContext = new ApplicationDbContext())
            {
                var FullLics = dbContext.NotSendReceipts.Where(x=>x.IsSend == false && x.TypeReceipt <= 7 && x.TypeReceipt == (int)TypeReceipt.PersonalReceipt).Select(x=>x.Lic).ToList();
                if(FullLics.Any())
                    _jobmanager.SendReceipt(String.Join(";", FullLics));
            }

           
            await Task.CompletedTask;
        }
    }
}
