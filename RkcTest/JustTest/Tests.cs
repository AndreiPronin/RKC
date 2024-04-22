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
        
        LinkedList<int> lists = new LinkedList<int>();
        [TestMethod]
        public void HashSetTest()
        {
           
            for (int l=0;l<10000000; l++)
            {
                lists.AddFirst(l);
            }
            
            Console.WriteLine("00000");
            using (var context = new DbTPlus())
            {
                var watcher1 = new Stopwatch();
                watcher1.Start();
                var z = lists.FirstOrDefault(x => x == 151236);
                lists.Remove(z);
                watcher1.Stop();
                var watcher2 = new Stopwatch();
                watcher2.Start();
                var z2 = lists.Where(x => x == 151237).ToList();
                lists.Remove(151237);
                watcher2.Stop();
                var t1 = watcher1.ElapsedMilliseconds;
                var t2 = watcher2.ElapsedMilliseconds;
            }
        }
    }
}
