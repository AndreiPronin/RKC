using BE.Counter;
using BE.Court;
using BE.http;
using BL.Extention;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.ApiServices.Counters
{
    public interface IApiCounters
    {
        Task<ResultResponse<string, List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, int? take, string lastLic = "");
        Task<ResultResponse<string, List<IpuGisReadingActive>>> GetIpuReadingsForGisActive(int? take, string lastLic = "");
        Task<ResultResponse<string, List<LicInfoForGis>>> GetLicInfoForGis(int? take, string lastLic = "", string els = "");
        Task<decimal?> GetReading(int IdPu);
        Task<List<FullLicByGisId>> GetFullLicBuGuidGis(List<string> gisId);
        Task UpdatePuWithGis(UpdatePuWithGis updatePuWithGis);
    }
    public sealed class ApiCounters : Repository, IApiCounters
    {
        public async Task<ResultResponse<string,List<IpuGisReading>>> GetIpuReadingsForGis(DateTime period, int? take, string lastLic = "")
        {
            var result = new ResultResponse<string, List<IpuGisReading>>();
            List<ALL_LICS_ARCHIVE> Allic = await GetALL_LICS_ARCHIVE(period, take, lastLic);

            var iPU_COUNTERsTask = await getIPU_COUNTERS(Allic.Select(x => x.F4ENUMELS).ToList(), period);
            var FlatMkdTask = await getFlatMkd(Allic.Select(x => x.F4ENUMELS).ToList());
            var AddressMKDsTask = await getAddressMKD(Allic.Select(x => (int)x.CADR).ToList());

            foreach (var item in iPU_COUNTERsTask)
            {
                var lic = Allic.FirstOrDefault(x => x.F4ENUMELS == item.FULL_LIC);
                var iPU_COUNTER = item.ConvertToIpuGisReading(lic,
                    AddressMKDsTask.FirstOrDefault(x => x.AddressId == (int)lic.CADR),
                    FlatMkdTask.FirstOrDefault(x => x.FullLic == item.FULL_LIC));
                if (iPU_COUNTER.FinalReadings != null)
                    result.value.Add(iPU_COUNTER);
            }
            result.lastId = Allic.LastOrDefault()?.F4ENUMELS;
            return result;
        }
        public async Task<ResultResponse<string, List<IpuGisReadingActive>>> GetIpuReadingsForGisActive(int? take, string lastLic = "")
        {
            var result = new ResultResponse<string, List<IpuGisReadingActive>>();
            List<ALL_LICS> Allic = await GetALL_LICS(take, lastLic);

            var iPU_COUNTERsTask = getIPU_COUNTERS(Allic.Select(x => x.F4ENUMELS).ToList(), null);
            var FlatMkdTask = getFlatMkd(Allic.Select(x => x.F4ENUMELS).ToList());
            var AddressMKDsTask = getAddressMKD(Allic.Select(x => (int)x.CADR).ToList());
            await Task.WhenAll(iPU_COUNTERsTask, FlatMkdTask, AddressMKDsTask);
            foreach (var item in iPU_COUNTERsTask.Result)
            {
                var lic = Allic.FirstOrDefault(x => x.F4ENUMELS == item.FULL_LIC);
                var iPU_COUNTER = item.ConvertToIpuGisReading(lic,
                    AddressMKDsTask.Result.FirstOrDefault(x => x.AddressId == (int)lic.CADR),
                    FlatMkdTask.Result.FirstOrDefault(x => x.FullLic == item.FULL_LIC));
                if(!string.IsNullOrEmpty(iPU_COUNTER.Fias) && !string.IsNullOrEmpty(iPU_COUNTER.UniqueApartmentNumber)
                   && !string.IsNullOrEmpty(iPU_COUNTER.Els) && !string.IsNullOrEmpty(iPU_COUNTER.IdGku))
                    result.value.Add(iPU_COUNTER);
            }
            result.lastId = Allic.LastOrDefault()?.F4ENUMELS;
            return result;
        }
        public async Task<ResultResponse<string, List<LicInfoForGis>>> GetLicInfoForGis(int? take, string lastLic = "",string els = "")
        {
            var result = new ResultResponse<string, List<LicInfoForGis>>();
           
            var FlatMkdTask = await getFlatMkd(take, lastLic, els);
            var Allic = await GetALL_LICS(FlatMkdTask.Select(x => x.FullLic).ToList());
            var AddressMKDsTask = await getAddressMKD(Allic.Select(x => (int)x.CADR).ToList());
            
            foreach (var item in Allic)
            {
                var flat = FlatMkdTask.Where(x => x.FullLic == item.F4ENUMELS).FirstOrDefault();
                result.value.Add(new LicInfoForGis
                {
                    IsClosed = item.ZAK,
                    AccountNumber = flat.FullLic,
                    TotalSquare = item.SOBS,
                    NumberOfPersons = item.KL,
                    Els = flat.Els,
                    AccountGUID = flat.AccountGUID,
                    Igku = flat.IdGku,
                    UnifiedAccountNumber = flat.UniqueApartmentNumber,
                    Firstname = item.FAMIL,
                    Surname = item.IMYA,
                    Patronymic = item.OTCH,

                });
            }
            result.lastId = Allic.LastOrDefault()?.F4ENUMELS;
            return result;
        }
        public async Task<List<FullLicByGisId>> GetFullLicBuGuidGis(List<string> gisId)
        {
            var result = await getFullLicBuGuidGis(gisId);
            return result;
        }
        public async Task UpdatePuWithGis(UpdatePuWithGis updatePuWithGis)
        {
            using (var context = new DbTPlus())
            {
                var ipu = context.IPU_COUNTERS.Find(updatePuWithGis.IdPu);
                ipu.GIS_ID_PU = updatePuWithGis.MeteringDeviceGISGKHNumber;
                await context.SaveChangesAsync();
            }
        }

        public async Task<decimal?> GetReading(int IdPu)
        {
            var ipueCounter = await getIpuCounters(IdPu);
            var lic = await GetALL_LICS(ipueCounter.FULL_LIC);
            return ipueCounter.ConvertToIpuGisReading(lic);
        }
    }
}
