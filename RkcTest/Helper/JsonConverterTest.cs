using BE.Counter;
using BL.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RkcTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.Helper
{
    [TestClass]
    public class JsonConverterTest
    {
        [TestMethod]
        public void JsonConverTest()
        {
            var convertJson = new ConvertJson<ModelAddPU>(ModelCreator.modelAddPU);
            var result1 = convertJson.ConverModelToJson();
            var result2 = convertJson.ConverJsonToModel(result1);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }
    }
}
