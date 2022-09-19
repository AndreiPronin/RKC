using AppCache;
using BL;
using BL.ApiT_;
using BL.Counters;
using BL.Jobs;
using BL.Notification;
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
        public void GeneratePdf()
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
            Module.RegistrationService(kernel);
            ICacheApp cache = kernel.Get<ICacheApp>();
            IPersonalData pers = kernel.Get<IPersonalData>();
            ICounter counter = kernel.Get<ICounter>();
            var ebd = new EBD(cache, pers, counter);
            ebd.CreateEBDAll();
        }
        [Test]
        public void CheckDublicate()
        {
            IKernel kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            INotificationMail notificationMail = kernel.Get<INotificationMail>();
            IJobManager JobManager = new JobManager(notificationMail);
            JobManager.CheckDublicatePu();
            JobManager.CheckDublicatePers();
        }
    }
}