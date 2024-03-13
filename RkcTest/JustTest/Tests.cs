using DB.DataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RkcTest.JustTest
{
    [TestClass]
    public class Tests
    {
        List<int> lists = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        [TestMethod]
        public void HashSetTest()
        {
           
            for (int l=0;l<10; l++)
            {
                Task.Run(() =>
                {
                    Console.WriteLine(lists[l]);
                });
            }
            
            Console.WriteLine("00000");
            using (var context = new DbTPlus())
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
