using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WordGenerator.Enums;
using WordGenerator;
using BL.ApiServices.PersonalData;
using RKC.Extensions;

namespace RKC.Controllers.api
{
    [RoutePrefix("api/v1/PersonalData")]
    public class PersonalDataController : ApiController
    {
        private readonly IApiPersonalData _apiPersonalData;
        public PersonalDataController(IApiPersonalData apiPersonalData) 
        {
            _apiPersonalData = apiPersonalData;
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("SendReseipt")]
        public string SendReseipt(string FullLic, string Email ,DateTime DateStart, DateTime DateEnd)
        {
            _apiPersonalData.SendReceipt(FullLic, Email, DateStart, DateEnd);
            return "Ok";
        }
    }
}
