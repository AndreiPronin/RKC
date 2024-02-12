using BE.Counter;
using BE.http;
using BL.ApiServices;
using BL.ApiServices.Counters;
using BL.Counters;
using BL.Helper;
using BL.http;
using BL.Security;
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
        public async Task<ResultResponse<string, List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, string lastId = "")
        {
            var result = await _apiCounters.GetIpuReadingsForGis(period, lastId);
            return result;
        }

    }
}
