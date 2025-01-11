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
using BL.Helper;
using BE.App;
//using NLog;

namespace BL.Jobs
{

    public static class Scheduler
    {
        // каждую субботу проверка
        const string cronCheck = @"0 0 0 ? * SAT *";
        private static string Mode;
        static Scheduler()
        {
            Mode =  new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.Mode).GetString();
        }
        //static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task Start()
        {
            if (!string.IsNullOrEmpty(Mode) && Mode == AppMode.Prod)
            {
                ShedulerLogger.WhriteToFile("Начало работы шедулера");
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail personalReceiptTrySend = JobBuilder.Create<JobSendReceipt>().Build();

                IJobDetail jobCheckSendReceipt = JobBuilder.Create<JobCheckEmailSender>().Build();
                IJobDetail jobClearIntegration = JobBuilder.Create<ClearIntegration>().Build();
                IJobDetail jobCacheUpdate = JobBuilder.Create<CacheUpdate>().Build();

                ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                    .WithIdentity("JobSender", "JobManagerGroup")     // идентифицируем триггер с именем и группой
                    .StartNow()                            // запуск сразу после начала выполнения
                    .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                        .WithIntervalInMinutes(10)         // через 6 часов
                        .RepeatForever())                   // бесконечное повторение
                    .Build();                               // создаем триггер
                ITrigger triggerCheckSendReceipt = TriggerBuilder.Create()  // создаем триггер
                   .WithIdentity("jobSendReceipt", "jobSendReceiptGroup")     // идентифицируем триггер с именем и группой
                   .WithCronSchedule(cronCheck)
                   .Build();                               // создаем триггер

                ITrigger triggerRemoveOldIntegration = TriggerBuilder.Create()  // создаем триггер
                   .WithIdentity("jobClearIntegration", "jobClearIntegrationGroup")     // идентифицируем триггер с именем и группой
                   .StartNow()                            // запуск сразу после начала выполнения
                   .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                       .WithIntervalInHours(12)          // через 12 часов
                       .RepeatForever())                   // бесконечное повторение
                   .Build();                               // создаем триггер

                ITrigger cacheUpdate = TriggerBuilder.Create()  // создаем триггер
                  .WithIdentity("cacheUpdate", "cacheUpdateGroup")     // идентифицируем триггер с именем и группой
                  .StartNow()                            // запуск сразу после начала выполнения
                  .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                      .WithIntervalInHours(1)          // через 12 часов
                      .RepeatForever())                   // бесконечное повторение
                  .Build();                               // создаем триггер

                scheduler.ScheduleJob(personalReceiptTrySend, trigger);        // начинаем выполнение работы
                scheduler.ScheduleJob(jobCheckSendReceipt, triggerCheckSendReceipt);        // начинаем выполнение работы

                scheduler.ScheduleJob(jobClearIntegration, triggerRemoveOldIntegration);        // начинаем выполнение работы

                scheduler.ScheduleJob(jobCacheUpdate, cacheUpdate);        // начинаем выполнение работы

                ShedulerLogger.WhriteToFile("Конец работы шедулера");
            }
        }
        
    }
}
