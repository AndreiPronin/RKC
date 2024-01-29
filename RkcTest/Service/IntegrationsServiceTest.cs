using AppCache;
using BL;
using BL.Counters;
using BL.Notification;
using BL.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
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
        public IntegrationsServiceTest() {
            var Kernel = new StandardKernel();
            Module.RegistrationService(Kernel);
            _cacheApp = Kernel.Get<ICacheApp>();
            _notificationMail = Kernel.Get<INotificationMail>();
            _counter = Kernel.Get<ICounter>();
        }
        [TestMethod]
        public async Task LoadReadingsTestAsync()
        {
            var integrationsTest = new Integrations();
            await integrationsTest.LoadReadings("NunitTest", _cacheApp, new DateTime(2024, 1, 15), _notificationMail, _counter, "", new DateTime(2024, 1, 15));
        }
    }
}
