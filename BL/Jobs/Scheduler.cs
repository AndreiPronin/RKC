using Castle.Core.Smtp;
using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BL.Loggers;
//using NLog;

namespace BL.Jobs
{

    public static class Scheduler
    {
        //static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task Start()
        {
            ShedulerLogger.WhriteToFile("Начало работы шедулера");
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail personalReceiptTrySend = JobBuilder.Create<JobSendReceipt>().Build();

            IJobDetail jobSendReceipt = JobBuilder.Create<JobEmailSender>().Build();
            IJobDetail jobClearIntegration = JobBuilder.Create<ClearIntegration>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("JobSender", "JobManagerGroup")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInMinutes(10)         // через 6 часов
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            ITrigger triggerSendReceipt = TriggerBuilder.Create()  // создаем триггер
               .WithIdentity("jobSendReceipt", "jobSendReceiptGroup")     // идентифицируем триггер с именем и группой
               .StartNow()                            // запуск сразу после начала выполнения
               .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                   .WithIntervalInHours(3)          // через 6 часов
                   .RepeatForever())                   // бесконечное повторение
               .Build();                               // создаем триггер

            ITrigger triggerRemoveOldIntegration = TriggerBuilder.Create()  // создаем триггер
               .WithIdentity("jobClearIntegration", "jobClearIntegrationGroup")     // идентифицируем триггер с именем и группой
               .StartNow()                            // запуск сразу после начала выполнения
               .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                   .WithIntervalInHours(12)          // через 12 часов
                   .RepeatForever())                   // бесконечное повторение
               .Build();                               // создаем триггер

            scheduler.ScheduleJob(personalReceiptTrySend, trigger);        // начинаем выполнение работы
            scheduler.ScheduleJob(jobSendReceipt, triggerSendReceipt);        // начинаем выполнение работы

            scheduler.ScheduleJob(jobClearIntegration, triggerRemoveOldIntegration);        // начинаем выполнение работы

            ShedulerLogger.WhriteToFile("Конец работы шедулера");
        }
        
    }
}
