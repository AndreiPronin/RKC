using DB.DataBase;
using DB.Model;
using DB.Model.Court.DictiomaryModel;
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
        Task<List<string>> GetCourtNameDictionaries(string Text, int Id);
        Task<List<CourtNameDictionary>> GetAllCourtNameDictionaries();
        Task<List<CourtValueDictionary>> GetCourtValueDictionaryId(int Id);
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
        public async Task<List<string>> GetCourtNameDictionaries(string Text, int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(Text))
                    return await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id).Select(x => x.Name).ToListAsync();
                var Result = await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id && x.Name.Contains(Text)).Select(x=>x.Name).ToListAsync();
                return Result;
            }
        }
        public async Task<List<CourtValueDictionary>> GetCourtValueDictionaryId(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id).ToListAsync();
                return Result;
            }
        }
        public async Task<List<CourtNameDictionary>> GetAllCourtNameDictionaries()
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtNameDictionaries.ToListAsync();
                return Result;
            }
        }
    }
}
