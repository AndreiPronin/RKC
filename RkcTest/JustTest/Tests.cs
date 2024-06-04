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
        string Json = @"{
    ""a"":10
},
[
]";
        
        LinkedList<int> lists = new LinkedList<int>();
        [TestMethod]
        public void HashSetTest()
        {
           var valid = new Stack<char>();
            var String = Json.Split('\n');
            foreach (var item in String)
            {
                if (item.StartsWith("{"))
                {
                    valid.Push(item[0]);
                }
            }
        }
    }
}
