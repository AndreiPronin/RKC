using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BL.Notification;
using WordGenerator;

namespace BL.Jobs
{
    public class JobEmailSender : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            IJobManager _jobmanager = new JobManager(new NotificationMail(), new ReceiptFactory());
            _jobmanager.CheckDublicatePu();
            _jobmanager.CheckDublicatePers();
            //await Task.CompletedTask;
        }
    }
}
