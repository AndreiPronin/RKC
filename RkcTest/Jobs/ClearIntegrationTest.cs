using BL.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.Jobs
{
    [TestClass]
    public class ClearIntegrationTest
    {
        [TestMethod]
        public void ClearIntegrationMethodTest()
        {
            var clearIntegration = new ClearIntegration();
            clearIntegration.Execute(new Mock<IJobExecutionContext>().Object);
        }
        [TestMethod]
        public void EmailSenderMethodTest()
        {
            var JobEmailSender = new JobEmailSender();
            JobEmailSender.Execute(new Mock<IJobExecutionContext>().Object);
        }
    }
}
