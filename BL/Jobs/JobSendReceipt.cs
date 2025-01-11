using BE.JobManager;
using BL.Loggers;
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
        public void Execute(IJobExecutionContext context)
        {
            IJobManager _jobmanager = new JobManager(new NotificationMail(), new ReceiptFactory());
            using(var dbContext = new ApplicationDbContext())
            {
                ShedulerLogger.WhriteToFile("Начало Отправки квитанций");
                var FullLics = dbContext.NotSendReceipts.Where(x=>x.IsSend == false && x.NumberAttempts <= 3 && x.TypeReceipt == (int)TypeReceipt.PersonalReceipt).Select(x=>x.Lic).ToList();
                ShedulerLogger.WhriteToFile($"Готово к отправке {FullLics.Count()}");
                if (FullLics.Any() && ((DateTime.Now.Hour > 4 && DateTime.Now.Hour > 8) || DateTime.Now.Hour < 4))
                    _jobmanager.SendReceipt(String.Join(";", FullLics));
            }
        }
    }
}
