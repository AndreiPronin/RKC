using AppCache;
using BL.ApiT_;
using BL.ApiServices.Counters;
using BL.Counters;
using BL.Excel;
using BL.Helper;
using BL.Jobs;
using BL.Notification;
using BL.Security;
using BL.Service;
using BL.Services;
using Ninject;
using WordGenerator;
using WordGenerator.interfaces;
using BL.Services.FileServices;
using BL.ApiServices.PersonalData;

namespace BL
{
    public static class Module
    {
        public static void RegistrationService(IKernel kernel)
        {
            kernel.Bind<ICounter>().To<Counter>();
            kernel.Bind<ICounterFileServices>().To<CounterFileServices>();
            kernel.Bind<Ilogger>().To<Logger>();
            kernel.Bind<IGeneratorDescriptons>().To<GeneratorDescriptons>();
            kernel.Bind<ICacheApp>().To<CacheApp>().InSingletonScope();
            kernel.Bind<IFlagsAction>().To<FlagsAction>().InSingletonScope();
            kernel.Bind<IPersonalData>().To<PersonalData>();
            kernel.Bind<IEBD>().To<EBD>();
            kernel.Bind<ISecurityProvider>().To<SecurityProvider>();
            kernel.Bind<IIntegrations>().To<Integrations>();
            kernel.Bind<IJobManager>().To<JobManager>();
            kernel.Bind<INotificationMail>().To<NotificationMail>();
            kernel.Bind<IExcel>().To<Excel.Excel>();
            kernel.Bind<ICourt>().To<Court>();
            kernel.Bind<IExcelDpu>().To<ExcelDpu>();
            kernel.Bind<IDpu>().To<Dpu>();
            kernel.Bind<IReport>().To<Report>();
            kernel.Bind<IBaseService>().To<BaseService>();
            kernel.Bind<IApiReportService>().To<ApiReportService>();
            kernel.Bind<IDictionary>().To<Dictionarys>();
            kernel.Bind<IMkdInformationService>().To<MkdInformationService>();

            kernel.Bind<IPdfGenerate>().To<ReceiptPersonal>().Named("Personal");
            kernel.Bind<IPdfGenerate>().To<ReceiptDPU>().Named("Dpu");
            kernel.Bind<IPdfFactory>().To<ReceiptFactory>();
            kernel.Bind<IExcelCourt>().To<ExcelCourt>();
            kernel.Bind<IExcelCourtReport>().To<ExcelCourtReport>();
            //kernel.Bind<IHttpClientFactory>().ToConstant(hostBuilder.Build().Services.GetService<IHttpClientFactory>());

            kernel.Bind<ITokenCreator>().To<TokenCreator>();


            //API
            kernel.Bind<IApiCounters>().To<ApiCounters>();
            kernel.Bind<IApiPersonalData>().To<ApiPersonalData>();

        }
    }
}
