using BL.Counters;
using BL.Helper;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RKC;
using System.Threading.Tasks;

namespace RKC_TEST.Service
{
    
    public class CounterServiceTest
    {
        Counter Counter { get; set; }
        [SetUp]
        public void Setup() 
        {
            //var kernel = NinjectWebCommon.CreateKernel();
            //Counter = new Counter(kernel.Get<Ilogger>(), kernel.Get<IGeneratorDescriptons>());
        }
        [Test]
        public async Task DetailInfromsAllAsync()
        {
            var result =  await Counter.DetailInfromsAllAsync();
            Assert.IsNotNull(result);
        }
    }
}
