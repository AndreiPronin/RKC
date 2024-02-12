using BE.Counter;
using BE.http;
using BL.Extention;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.ApiServices.Counters
{
    public interface IApiCounters
    {
        Task<ResultResponse<string, List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, string lastLic = "");
    }
    public class ApiCounters : Repository, IApiCounters
    {
        public async Task<ResultResponse<string,List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, string lastLic = "")
        {
            var result = new ResultResponse<string, List<IpuGisReading>>();
            List<ALL_LICS_ARCHIVE> Allic = await GetALL_LICS_ARCHIVE(period,lastLic);
     
            var iPU_COUNTERsTask = getIPU_COUNTERS(Allic.Select(x=>x.F4ENUMELS).ToList());
            var FlatMkdTask = getFlatMkd(Allic.Select(x => x.F4ENUMELS).ToList());
            var AddressMKDsTask = getAddressMKD(Allic.Select(x => (int)x.CADR).ToList());
            await Task.WhenAll(iPU_COUNTERsTask, FlatMkdTask, AddressMKDsTask);
            foreach (var item in iPU_COUNTERsTask.Result) 
            {
                var lic = Allic.FirstOrDefault(x => x.F4ENUMELS == item.FULL_LIC);
                var iPU_COUNTER = item.ConvertToIpuGisReading(lic,
                    AddressMKDsTask.Result.FirstOrDefault(x => x.AddressId == (int)lic.CADR),
                    FlatMkdTask.Result.FirstOrDefault(x => x.FullLic == item.FULL_LIC));
                if (iPU_COUNTER.FinalReadings != null)
                    result.value.Add(iPU_COUNTER);
            }
            result.lastId = Allic.LastOrDefault()?.F4ENUMELS;

            return result;
        }
        
    }
}
