using BL.ApiT_;
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
    public class VSController : ApiController
    {
        private readonly IEBD _ebd;
        public VSController(IEBD ebd)
        {
            _ebd = ebd;
        }
        [HttpGet]
        public HttpResponseMessage GetEbdXmlForTplus(DateTime? dateFrom, DateTime? dateTill)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_ebd.CreateEBDAll(dateFrom.Value,dateTill.Value))
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ebd.xml"
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
