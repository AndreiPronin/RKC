using BE.Counter;
using BE.Court;
using BE.http;
using BL.ApiServices;
using BL.ApiServices.Counters;
using BL.Counters;
using BL.Helper;
using BL.http;
using BL.Security;
using DocumentFormat.OpenXml.Drawing.Charts;
using NLog;
using RKC.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;


namespace RKC.Controllers.api
{
    [RoutePrefix("api/v1/Counter")]
    public class CounterController : ApiController
    {
        public NLog.Logger _Nlogger = LogManager.GetCurrentClassLogger();
        private readonly ICounter _counter;
        private readonly IApiCounters _apiCounters;
        public CounterController(ICounter counter, IApiCounters apiCounters) 
        { 
            _counter = counter;
            _apiCounters = apiCounters;
        }
        [JwtAuthentication]
        [HttpPost]
        [Route("UpdateGuidPuWithGis")]
        public IEnumerable<ConnectPuWithGisResponse> UpdateGuidPuWithGis(IEnumerable<ConnectPuWithGis> model)
        {
            _Nlogger.Info($"Обновление гуидов ПУ: {new ConvertJson<List<ConnectPuWithGis>>(model.ToList()).ConverModelToJson()}");
            return _counter.UpdateGuidPuWithGis(model);
         
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("GetIpuReadingsForGis")]
        public async Task<ResultResponse<string, List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, int? take, string lastId = "")
        {
            var result = await _apiCounters.GetIpuReadingsForGis(period,take, lastId);
            return result;
        }
        //[JwtAuthentication]
        [HttpGet]
        [Route("GetLicInfoForGis")]
        public async Task<ResultResponse<string, List<LicInfoForGis>>> GetLicInfoForGis(int? take, string lastId = "", string els = "")
        {
            var result = await _apiCounters.GetLicInfoForGis(take, lastId,els);
            return result;
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("GetIpuReadingsForGisActive")]
        public async Task<ResultResponse<string, List<IpuGisReadingActive>>> GetIpuReadingsForGisActive(int? take, string lastId = "")
        {
            var result = await _apiCounters.GetIpuReadingsForGisActive(take, lastId);
            return result;
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("GetFullLicBuGuidGis")]
        public async Task<List<FullLicByGisId>> GetFullLicBuGuidGis(List<string> gisId)
        {
            var result = await _apiCounters.GetFullLicBuGuidGis(gisId);
            return result;
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("GetReading")]
        public async Task<decimal?> GetReading(int IdPu)
        {
            var result = await _apiCounters.GetReading(IdPu);
            return result;
        }
        [JwtAuthentication]
        [HttpPost]
        [Route("UpdatePuWithGis")]
        public async Task<HttpResponseMessage> UpdatePuWithGis(UpdatePuWithGis updatePuWithGis)
        {
            try
            {
                await _apiCounters.UpdatePuWithGis(updatePuWithGis);
                return new HttpResponseMessage() { 
                    StatusCode = System.Net.HttpStatusCode.OK 
                };
            }catch (Exception ex)
            {
                return new HttpResponseMessage() { 
                    StatusCode = System.Net.HttpStatusCode.InternalServerError, 
                    Content = new StringContent(ex.Message) 
                };
            };
        }
    }
}
