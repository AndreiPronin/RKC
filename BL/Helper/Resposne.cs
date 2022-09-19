using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public static class Resposne
    {
        public static HttpResponseMessage CreateResponse200()
        {
            return new HttpResponseMessage(HttpStatusCode.OK); ;
        }
    }
}
