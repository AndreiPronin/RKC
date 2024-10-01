using BE.JobManager;
using BE.Roles;
using BL.ApiT_;
using BL.Helper;
using BL.http;
using BL.Jobs;
using NLog;
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
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public ApiJobController(IJobManager job)
        {
            _job = job;
        }
        [HttpGet]
        public HttpResponseMessage RunJob(int id)
        {
            switch (id)
            {
                case (int)EnumJob.CheckDublicatePu:
                    _job.CheckDublicatePu();
                    break;
                case (int)EnumJob.CheckDublicatePers:
                    _job.CheckDublicatePers();
                    break;
                case (int)EnumJob.SendReceipt:

                    if (User.IsInRole(RolesEnums.SuperAdmin))
                    {
                        logger.Info($"{User.Identity.Name} запустил массовую рассылку");
                        _job.SendReceipt();
                    }
                    break;
                case (int)EnumJob.SendReceiptDpu:
                    if (User.IsInRole(RolesEnums.DPUAdmin) || User.IsInRole(RolesEnums.SuperAdmin))
                    {
                        logger.Info($"{User.Identity.Name} запустил массовую рассылку ДПУ");
                        _job.SendReceiptDpu();
                    }
                    break;
                case (int)EnumJob.CheckDublicatePuNumberPu:
                    if (User.IsInRole(RolesEnums.DPUAdmin) || User.IsInRole(RolesEnums.SuperAdmin))
                        _job.CheckDublicatePuNumber();
                    break;
                default:
                    break;
            }
            return Resposne.CreateResponse200();
        }
        [Route("api/SendReceiptLic")]
        [HttpPost]
        public HttpResponseMessage SendReceiptLic(SendReceiptLic sendReceiptLic)
        {
            if (User.IsInRole(RolesEnums.DownLoadReceipt) || User.IsInRole(RolesEnums.SuperAdmin) || User.IsInRole(RolesEnums.Admin))
            {
                logger.Info($"{User.Identity.Name} запустил рассылку. Лицевые счат {string.Join("", sendReceiptLic.FullLic)}");
                _job.SendReceipt(sendReceiptLic.FullLic);
            }
            return Resposne.CreateResponse200();
        }
        [Route("api/SendReceiptLicDpu")]
        [HttpPost]
        public HttpResponseMessage SendReceiptLicDpu(SendReceiptLic sendReceiptLic)
        {
            if (User.IsInRole(RolesEnums.DownLoadReceipt) || User.IsInRole(RolesEnums.SuperAdmin) || User.IsInRole(RolesEnums.Admin))
            {
                logger.Info($"{User.Identity.Name} запустил рассылку ДПУ. Лицевые счат {string.Join("", sendReceiptLic.FullLic)}");
                _job.SendReceiptDpu(sendReceiptLic.FullLic);
            }
            return Resposne.CreateResponse200();
        }
    }
}
