﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.http
{
    public static class Resposne
    {
        public static HttpResponseMessage CreateResponse200()
        {
            return new HttpResponseMessage(HttpStatusCode.OK); ;
        }
        public static HttpResponseMessage CreateResponse200(string Message)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Message);
            return response;
        }
        public static HttpResponseMessage CreateResponse400()
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest); ;
        }
        public static HttpResponseMessage CreateResponse500(string Message)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(Message);
            return response;
        }
    }
}
