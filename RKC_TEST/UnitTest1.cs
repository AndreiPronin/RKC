using BL.Counters;
using BL.Helper;
using DB.Model;
using Moq;
using Ninject;
using NUnit.Framework;
using RKC.App_Start;
using RKC.Controllers;
using AppCache;
using System.Web;

namespace RKC_TEST
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            BL.Services.PersonalData personalData = new BL.Services.PersonalData();
            personalData.GetInfoPersData("724097769");

        }


    }
   
}