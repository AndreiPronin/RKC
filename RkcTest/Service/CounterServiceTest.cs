using BL.Counters;
using BL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RKC.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Moq;

namespace RkcTest.Service
{
    [TestClass]
    public class CounterServiceTest
    {
        Counter Counter { get; set; }
        public CounterServiceTest()
        {
            var kernel = NinjectWebCommon.CreateKernel();
            Counter = new Counter(kernel.Get<Ilogger>(), kernel.Get<IGeneratorDescriptons>());
        }
        [TestMethod]
        public async Task DetailInfromsAllAsync()
        {
            var result = await Counter.DetailInfromsAllAsync();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task UpdateReadings()
        {
            
            await Counter.UpdateLicReadings(new Mock<BE.Counter.SaveModelIPU>().Object);
        }
        [TestMethod]
        public async Task GetFlatTypeLic()
        {
            await Task.CompletedTask;
            var res = Counter.GetFlatTypeLic("702019511");
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public async Task GetStatusCloseOpenLic()
        {
            await Task.CompletedTask;
            var res = Counter.GetStatusCloseOpenLic("702019511");
            Assert.IsFalse(res);
        }
    }
}
