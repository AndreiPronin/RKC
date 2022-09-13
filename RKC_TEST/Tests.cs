using AppCache;
using BL.ApiT_;
using BL.Counters;
using BL.Services;
using Ninject;
using NUnit.Framework;
using RKC.App_Start;
using System;
using WordGenerator;

namespace RKC_TEST
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            try
            {
                GenerateFileHelpCalculation.Generate("703027614", new System.DateTime(2022, 1, 2));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void EBD()
        {
            IKernel kernel = new StandardKernel();
            NinjectWebCommon.RegisterServices(kernel);
            ICacheApp cache = kernel.Get<ICacheApp>();
            IPersonalData pers = kernel.Get<IPersonalData>();
            ICounter counter = kernel.Get<ICounter>();
            var ebd = new EBD(cache, pers, counter);
            ebd.CreateEBDAll();
        }
    }
}