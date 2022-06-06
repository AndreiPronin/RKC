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
    public interface IPersonalData
    {
        List<PersData> GetInfoPersData(string FullLic);
    }
    public class PersonalData : IPersonalData
    {
        public List<PersData> GetInfoPersData(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

                    var Res = db.PersData.Where(x => x.Lic == FullLic && x.IsDelete == false).Include("PersDataDocument").ToList();
                    return Res;
            }
            //using (DBPersData dBPersData = new DBPersData())
            //{
            //    IQueryable<PersData> query = dBPersData.PersData;
            //    return query.Where(x =>x.lic == FullLic && x.delete != false).ToList();
            //}
        }
    }
}
