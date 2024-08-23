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
            var ttt = DateTime.Now.AddMonths(1);
            var res = DateTime.Now.GetDateWhitMaxDate();
        }
        [TestMethod]
        public void GetYear_Test()
        {
            var result = DateTimes.GetYear(1);
            Assert.AreEqual(result, "1 год");
            result = DateTimes.GetYear(2);
            Assert.AreEqual(result, "2 года");
            result = DateTimes.GetYear(3);
            Assert.AreEqual(result, "3 года");
            result = DateTimes.GetYear(4);
            Assert.AreEqual(result, "4 года");
            result = DateTimes.GetYear(5);
            Assert.AreEqual(result, "5 лет");
            result = DateTimes.GetYear(24);
            Assert.AreEqual(result, "24 года");
        }
    }
}
