using BL.Notification;
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
           
           await Task.CompletedTask;
        }
    }
}
