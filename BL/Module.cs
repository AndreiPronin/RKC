using AppCache;
using BL.ApiT_;
using BL.Counters;
using BL.Excel;
using BL.Helper;
using BL.Jobs;
using BL.Notification;
using BL.Security;
using BL.Service;
using BL.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class Module
    {
        public static void RegistrationService(IKernel kernel)
        {
            kernel.Bind<ICounter>().To<Counter>();
            kernel.Bind<Ilogger>().To<Logger>();
            kernel.Bind<IGeneratorDescriptons>().To<GeneratorDescriptons>();
            kernel.Bind<ICacheApp>().To<CacheApp>().InSingletonScope();
            kernel.Bind<IFlagsAction>().To<FlagsAction>().InSingletonScope();
            kernel.Bind<IReadFileBank>().To<ReadFileBank>();
            kernel.Bind<IPersonalData>().To<PersonalData>();
            kernel.Bind<IEBD>().To<EBD>();
            kernel.Bind<ISecurityProvider>().To<SecurityProvider>();
            kernel.Bind<IIntegrations>().To<Integrations>();
            kernel.Bind<IJobManager>().To<JobManager>();
            kernel.Bind<INotificationMail>().To<NotificationMail>();
            kernel.Bind<IExcel>().To<Excel.Excel>();
            kernel.Bind<ICourt>().To<Court>();
        }
    }
}
