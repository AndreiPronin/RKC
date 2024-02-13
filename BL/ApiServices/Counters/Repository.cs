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
                var Allic = await contextAllic.ALL_LICS_ARCHIVE.Where(x => x.period == period && x.F4ENUMELS.CompareTo(lastLic ?? "") > 0).Take(take ?? 500).ToListAsync();
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
        protected async Task<List<IPU_COUNTERS>> getIPU_COUNTERS(List<string> Allic)
        {
            using (var contextTPlus = new DbTPlus())
            {
                return await contextTPlus.IPU_COUNTERS.Where(x => x.CLOSE_ != true
                && (x.FACTORY_NUMBER_PU != null || x.FACTORY_NUMBER_PU != "")
                && (x.BRAND_PU != null || x.BRAND_PU != "")
                && Allic.Contains(x.FULL_LIC)).ToListAsync(); ;
            }
        }
    }
}
