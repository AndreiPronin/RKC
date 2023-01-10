using AppCache;
using BE.PersData;
using DB.DataBase;
using DB.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public class ExaminationToExcel
    {
        public DataTable ExaminationPersIsLic(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "Получаю данные из бд");
            DataTable dt = new DataTable("Персы");
            List<PersNotFoundInLic> persNotFoundInLics = new List<PersNotFoundInLic>();
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"), new DataColumn("Ссылка")});
            var DB = new DbTPlus();
            using (var DbLIC = new DbLIC())
            {
                var AllLic = DbLIC.ALL_LICS.Select(x=>x.F4ENUMELS).ToList();
                using(var AppDb = new ApplicationDbContext())
                {
                    var Pers = AppDb.PersData.Where(x=>x.IsDelete != true).Select(x=>x.Lic).ToList();
                    foreach(var Item in AllLic)
                    {
                        var pers = Pers.FirstOrDefault(x => x == Item);
                        if (string.IsNullOrEmpty(pers))
                        {
                            persNotFoundInLics.Add(new PersNotFoundInLic
                            {
                                Lic = Item,
                                Href = $"{ConfigurationManager.AppSettings["App:Host"]}/PersonalData/PersonalInformation?FullLic={Item}"
                            });
                        }
                    }
                }
            }
            cacheApp.Update(User, "Формирую Excel");
            foreach (var Items in persNotFoundInLics)
            {
                dt.Rows.Add(Items.Lic, Items.Href);
            }
            cacheApp.Update(User, "Скачиваю Excel");

            return dt;
        }
    }
}
