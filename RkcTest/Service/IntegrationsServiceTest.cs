using AppCache;
using AutoMapper;
using BL;
using BL.Counters;
using BL.Notification;
using BL.Service;
using BL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using RKC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.Service
{
    [TestClass]
    public class IntegrationsServiceTest
    {
        private readonly ICacheApp _cacheApp;
        private readonly INotificationMail _notificationMail;
        private readonly ICounter _counter;
        private readonly IMkdInformationService _mkdInformationService;
        private readonly IMapper _mapper;
        public IntegrationsServiceTest() {
            var Kernel = new StandardKernel();
            Module.RegistrationService(Kernel);
            new AutoMapperModule().RegisterServices(Kernel);
            _cacheApp = Kernel.Get<ICacheApp>();
            _notificationMail = Kernel.Get<INotificationMail>();
            _counter = Kernel.Get<ICounter>();
            _mapper = Kernel.Get<IMapper>();
            _mkdInformationService = Kernel.Get<IMkdInformationService>();
        }
        [TestMethod]
        public async Task LoadReadingsTestAsync()
        {
            var integrationsTest = new Integrations(_mapper, _mkdInformationService);
            await integrationsTest.LoadReadings("NunitTest", _cacheApp, new DateTime(2024, 1, 15), _notificationMail, _counter, "", new DateTime(2023, 1, 15));
        }
    }
}
