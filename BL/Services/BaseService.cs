using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IBaseService
    {
        /// <summary>
        /// Информация заркыт или открыт лицевой счет
        /// </summary>
        /// <param name="FullLic"></param>
        /// <returns></returns>
        bool GetStatusCloseOpenLic(string FullLic);
        Task ClosePersInLic(string FullLic);
    }
    public class BaseService : IBaseService
    {
        public bool GetStatusCloseOpenLic(string FullLic)
        {
            using (var dbLic =new DbLIC())
            {
                var Zak = dbLic.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == FullLic);
                if(Zak != null && Zak.ZAK != null)
                {
                    return true;
                }
                if (Zak != null && Zak.ZAK == null)
                {
                    return false;
                }
                return false;
            }
        }
        public async Task ClosePersInLic(string FullLic)
        {
            using (var dbfSql = new DbLIC())
            {
                var Lic = await dbfSql.ALL_LICS.FirstOrDefaultAsync(x => x.F4ENUMELS == FullLic);
                Lic.SOBS = 0;
                Lic.KL = 0;
                await dbfSql.SaveChangesAsync();
            }
        }
    }
}
