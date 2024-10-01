using BL;
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
    public class MkdInformationServiceTest
    {
        private readonly IMkdInformationService _mkdInformationService;
        public MkdInformationServiceTest()
        {
            var kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            new AutoMapperModule().RegisterServices(kernel);
            _mkdInformationService = kernel.Get<IMkdInformationService>();
        }
        [TestMethod]
        public void GetAddressMKD_Test()
        {
            var result = _mkdInformationService.GetAddressMKD("702019376");
            Assert.IsNotNull(result);
            result = _mkdInformationService.GetAddressMKD("");
            Assert.IsNull(result);
        }
    }
}
