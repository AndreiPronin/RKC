using BE.JobManager;
using BL.ApiT_;
using BL.Helper;
using BL.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace RKC.Controllers
{
    public class ApiJobController : ApiController
    {
        private readonly IJobManager _job;
        public ApiJobController(IJobManager job)
        {
            _job = job;
        }
        [HttpGet]
        public HttpResponseMessage RunJob(int id)
        {
            var rr = User.Identity.Name;
            switch (id)
            {
                case (int)EnumJob.CheckDublicatePu:
                    _job.CheckDublicatePu();
                    break;
                case (int)EnumJob.CheckDublicatePers:
                    _job.CheckDublicatePers();
                    break;
                case (int)EnumJob.SendReceipt:
                    if(User.IsInRole("SuperAdmin"))
                        _job.SendReceipt();
                    break;
                default:
                    break;
            }
            return Resposne.CreateResponse200();
        }
        [Route("api/SendReceiptLic")]
        [HttpGet]
        public HttpResponseMessage SendReceiptLic(string FullLic)
        {
            var Lic = FullLic.Split(';');
            for(int i = 0; i < Lic.Length; i++)
                if (!string.IsNullOrEmpty(Lic[i].Trim()))
                    _job.SendReceipt(Lic[i].Trim());
            return Resposne.CreateResponse200();
        }
    }
}
