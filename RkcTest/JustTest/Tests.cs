using DB.DataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.JustTest
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void HashSetTest()
        {
            using(var context = new DbTPlus())
            {
                var counters = context.IPU_COUNTERS.ToList();
                var watcher1 = new Stopwatch();
                watcher1.Start();
                var z = counters.FirstOrDefault(x => x.FACTORY_NUMBER_PU == "22011909");
                watcher1.Stop();
                var watcher2 = new Stopwatch();
                watcher2.Start();
                var y = counters.Where(x => x.FACTORY_NUMBER_PU == "22011909").Select(x=>x.FACTORY_NUMBER_PU).ToHashSet();
                watcher2.Stop();
            }
        }
    }
}
