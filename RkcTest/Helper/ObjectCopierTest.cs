using DB.DataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Util;
using DB.Model;

namespace RkcTest.Helper
{
    [TestClass]
    public class ObjectCopierTest
    {
        [TestMethod]
        public void CloneTest()
        {
            using (var context = new DbTPlus())
            {
                var counter = context.IPU_COUNTERS.FirstOrDefault();
                var t = ObjectCopier.Clone(counter);
                counter.ID_PU = 20;
                t.ID_PU = 10;
                Assert.AreNotEqual(counter.ID_PU, t.ID_PU);
            }
        }
    }
}
