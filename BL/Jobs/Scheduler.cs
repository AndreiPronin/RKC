using Castle.Core.Smtp;
using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Jobs
{

    public class Scheduler
    {
        public static async Task Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobEmailSender>().Build();

            IJobDetail jobSendReceipt = JobBuilder.Create<JobEmailSender>().Build();
            IJobDetail jobSimple = JobBuilder.Create<SimpleJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("JobSender", "JobManagerGroup")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInHours(6)          // через 6 часов
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
               .WithIdentity("jobSimple", "jobSimpleGroup")     // идентифицируем триггер с именем и группой
               .StartNow()                            // запуск сразу после начала выполнения
               .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                   .WithIntervalInHours(12)          // через 12 часов
                   .RepeatForever())                   // бесконечное повторение
               .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
            await scheduler.ScheduleJob(jobSendReceipt, triggerSendReceipt);        // начинаем выполнение работы

            await scheduler.ScheduleJob(jobSimple, triggerRemoveOldIntegration);        // начинаем выполнение работы
        }
    }
}
