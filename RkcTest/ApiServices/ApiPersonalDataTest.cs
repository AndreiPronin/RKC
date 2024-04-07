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
using BL.ApiServices.PersonalData;

namespace RkcTest.ApiServices
{
    [TestClass]
    public class ApiPersonalDataTest
    {
        private readonly IApiPersonalData _apiPersonalData;
        public ApiPersonalDataTest() {
            var kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            _apiPersonalData = kernel.Get<IApiPersonalData>();

        }
        [TestMethod]
        public void SendReceiptTest()
        {
             _apiPersonalData.SendReceipt("721074364","andrei985623147@yandex.ru", new DateTime(2024, 03, 01), new DateTime(2024, 03, 01));
        }
        
    }
}
