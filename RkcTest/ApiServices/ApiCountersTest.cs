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
using BE.Counter;

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
        [TestMethod]
        public async Task GetIpuReadingsForGisActiveTest()
        {
            var ipuGisReadingActives = new List<IpuGisReadingActive>();
            var lastLic = "";
            while (true) 
            { 
                var result = await _apiCounters.GetIpuReadingsForGisActive(1000, lastLic);
                ipuGisReadingActives.AddRange(result.value);
                lastLic = result.lastId;
                if (ipuGisReadingActives.Where(x => x.FulLic == "720128131").Count() > 1)
                {
                    var zzz = ipuGisReadingActives.Where(x => x.FulLic == "720128131").ToList();
                }
                if(string.IsNullOrEmpty(lastLic))
                    break;
            }
            Assert.IsNotNull(ipuGisReadingActives);
        }
        [TestMethod]
        public async Task GetFullLicBuGuidGis()
        {
            var fullLicByGisId = new List<string>() { "8Myc02145600037", "2Myc02145600038" };
            var result = await _apiCounters.GetFullLicBuGuidGis(fullLicByGisId);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetIpuReadingWithClosePUForGisTest()
        {
            var result = await _apiCounters.GetIpuReadingsForGisActive(1000, "704093663");
            Assert.IsNotNull(result);
        }
    }
}
