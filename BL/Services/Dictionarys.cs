using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IDictionary
    {
        Task<List<DIMENSION>> GetDIMENSION();
    }
    public class Dictionarys : IDictionary
    {
        /// <summary>
        /// Справочник едениц измерений ПУ
        /// </summary>
        /// <returns></returns>
        public async Task<List<DIMENSION>> GetDIMENSION()
        {
            using (var db = new DbTPlus())
                return await db.DIMENSIONs.ToListAsync();
        }
    }
}
