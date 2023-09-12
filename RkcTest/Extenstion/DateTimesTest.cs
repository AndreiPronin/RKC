using BL.Extention;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.Extenstion
{
    [TestClass]
    public class DateTimesTest
    {
        [TestMethod]
        public void GetDateWhitMaxDate_Test()
        {
            var res = DateTime.Now.GetDateWhitMaxDate();
        }
    }
}
