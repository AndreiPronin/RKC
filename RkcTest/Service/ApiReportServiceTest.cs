using BL;
using BL.Services;
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
    public class ApiReportServiceTest
    {
        private readonly IApiReportService _apiReportService;
        public ApiReportServiceTest() {
            var kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            _apiReportService = kernel.Get<IApiReportService>();
        }
        [TestMethod]
        public async Task GetSberbankInvoicesOldFormatTest()
        {
            var result = await _apiReportService.GetSberbankInvoicesOldFormat(new DateTime(2023, 12, 1));
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetSberbankInvoicesTest()
        {
            var result = await _apiReportService.GetSberbankInvoices(new DateTime(2023, 12, 1));
            Assert.IsNotNull(result);
        }

    }
}
