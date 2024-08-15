using DB.DataBase;
using DB.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
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
 
        [TestMethod]
        public void HashSetTest()
        {
           var watcher = new Stopwatch();
            var watcher2 = new Stopwatch();
            watcher.Start();
            using(var context = new DbLIC())
            {
                var result = context.ALL_LICS.Where(x=>x.UL.Contains("Володар")).ToList();
            }
            watcher.Stop();
            watcher2.Start();
            using (var context = new DbLIC())
            {
                var result = context.ALL_LICS.Where(x => x.UL.Contains("Володар")).AsQueryable().ToList();
                
            }
            watcher2.Stop();

        }
    }
}
