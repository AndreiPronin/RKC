using BL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.ApiServices;
using System.Diagnostics;
using BL.ApiServices.Counters;

namespace RkcTest.ApiServices
{
    [TestClass]
    public class ApiCountersTest
    {
        private readonly IApiCounters _apiCounters;
        public ApiCountersTest() {
            var kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            _apiCounters = kernel.Get<IApiCounters>();

        }
        [TestMethod]
        public async Task GetIpuReadingsForGisTest()
        {

            var result =   await _apiCounters.GetIpuReadingsForGis(new DateTime(2023, 12, 31),1000, "");
            Assert.IsNotNull(result);
        }
    }
}
