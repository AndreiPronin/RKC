using BE.Counter;
using BE.Court;
using Castle.Core.Internal;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ApiServices.Counters
{
    public class Repository
    {
        protected async Task<List<ALL_LICS_ARCHIVE>> GetALL_LICS_ARCHIVE(DateTime period,int? take, string lastLic = "")
        {
            using (var contextAllic = new DbLIC())
            {
                var Allic = await contextAllic.ALL_LICS_ARCHIVE
                    .Where(x => x.period == period && x.F4ENUMELS.CompareTo(lastLic ?? "") > 0 && x.ZAK == null && !x.KW.ToUpper().StartsWith("Н"))
                    .OrderBy(x => x.F4ENUMELS)
                    .Take(take ?? 500)
                    .ToListAsync();
                return Allic;
            }
        }
        protected async Task<List<ALL_LICS>> GetALL_LICS( int? take, string lastLic = "")
        {
            using (var contextAllic = new DbLIC())
            {
                var Allic = await contextAllic.ALL_LICS
                    .Where(x => x.F4ENUMELS.CompareTo(lastLic ?? "") > 0 && x.ZAK == null && !x.KW.ToUpper().StartsWith("Н"))
                    .OrderBy(x=>x.F4ENUMELS)
                    .Take(take ?? 500)
                    .ToListAsync();
                return Allic;
            }
        }
        protected async Task<List<AddressMKD>> getAddressMKD(List<int> cadr)
        {
            using (var contextTPlus = new DbTPlus())
            {
                return await contextTPlus.addresses.Where(x => cadr.Contains(x.AddressId)).ToListAsync();
            }
        }
        protected async Task<List<FlatMkd>> getFlatMkd(List<string> Allic)
        {
            using (var contextTPlus = new DbTPlus())
            {
                return await contextTPlus.flats.Where(x => Allic.Contains(x.FullLic)).ToListAsync(); ;
            }
        }
        protected async Task<List<IPU_COUNTERS>> getIPU_COUNTERS(List<string> Allic, DateTime? period)
        {
            using (var contextTPlus = new DbTPlus())
            {
                List<IPU_COUNTERS> allPu = await contextTPlus.IPU_COUNTERS.Where(x =>
                    !string.IsNullOrEmpty(x.FACTORY_NUMBER_PU) && Allic.Contains(x.FULL_LIC))
                    .OrderBy(x => x.FULL_LIC).ThenBy(x => x.TYPE_PU).ToListAsync();

                Dictionary<string, IPU_COUNTERS> validPu = allPu.Where(x => x.CLOSE_ != true)
                          .ToDictionary(x => string.Concat(x.FULL_LIC, x.TYPE_PU), y => y);
                Dictionary<string, List<IPU_COUNTERS>> closedPu = allPu.Where(x => x.CLOSE_ == true)
                          .GroupBy(x => string.Concat(x.FULL_LIC, x.TYPE_PU))
                          .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DATE_CLOSE).ThenByDescending(x => x.ID_PU).ToList());

                foreach (var pair in closedPu)
                {
                    if (validPu.ContainsKey(pair.Key))
                        continue;

                    if (pair.Value.Count > 0)
                    {
                        validPu.Add(pair.Key, pair.Value[0]);
                    }
                }

                return validPu.Values.OrderBy(x => x.FULL_LIC).ThenBy(x => x.TYPE_PU).ToList();
            }
        }
        protected async Task<List<FullLicByGisId>> getFullLicBuGuidGis(List<string> gisId)
        {
            using (var contextTPlus = new DbTPlus())
            {
                var result =  await contextTPlus.IPU_COUNTERS.Where(x => gisId.Contains(x.GIS_ID_PU))
                    .Select(x=> new FullLicByGisId { FullLic = x.FULL_LIC, GisId = x.GIS_ID_PU})
                    .ToListAsync();
                return result;
            }
        }
        protected async Task UpdatePuWithGis(UpdatePuWithGis updatePuWithGis)
        {
            using (var context = new DbTPlus())
            {
                var ipu = context.IPU_COUNTERS.Find();
                await context.SaveChangesAsync();
            }
        }
    }
}
