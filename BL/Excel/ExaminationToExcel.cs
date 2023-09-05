using AppCache;
using BE.Counter;
using BE.PersData;
using DB.DataBase;
using DB.Model;
using DB.ViewModel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using BL.Extention;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public class CheckToExcel
    {
        public DataTable PersIsLic(string User, ICacheApp cacheApp)
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
        public DataTable PuIsLic(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "Получаю данные из бд");
            DataTable dt = new DataTable("Персы");
            List<IpuNotFoundInLick> ipuNotFoundInLick = new List<IpuNotFoundInLick>();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Лицевой счет"), new DataColumn("Тип ПУ"), new DataColumn("Описание"), new DataColumn("Ссылка") });
            var DB = new DbTPlus();
            using (var DbLIC = new DbLIC())
            {
                var AllLic = DbLIC.ALL_LICS.Select(x => new {
                    F4ENUMELS = x.F4ENUMELS,
                    FKUB2XVS = x.FKUB2XVS,
                    FKUBSXVS = x.FKUBSXVS,
                    FKUBSXV_2 = x.FKUBSXV_2,
                    FKUBSXV_3 = x.FKUBSXV_3,
                    FKUBSXV_4 = x.FKUBSXV_4,
                    FKUBSOT_1 = x.FKUBSOT_1,
                    FKUBSOT_2 = x.FKUBSOT_2,
                    FKUBSOT_3 = x.FKUBSOT_3,
                    FKUBSOT_4 = x.FKUBSOT_4,
                    FKUB1XVS = x.FKUB1XVS,
                    FKUB2XV_2 = x.FKUB2XV_2,
                    FKUB1XV_2 = x.FKUB1XV_2,
                    FKUB2XV_3 = x.FKUB2XV_3,
                    FKUB1XV_3 = x.FKUB1XV_3,
                    FKUB2XV_4 = x.FKUB2XV_4,
                    FKUB1XV_4 = x.FKUB1XV_4,
                    FKUB2OT_1 = x.FKUB2OT_1,
                    FKUB1OT_1 = x.FKUB1OT_1,
                    FKUB2OT_2 = x.FKUB2OT_2,
                    FKUB1OT_2 = x.FKUB1OT_2,
                    FKUB2OT_3 = x.FKUB2OT_3,
                    FKUB1OT_3 = x.FKUB1OT_3,
                    FKUB2OT_4 = x.FKUB2OT_4,
                    FKUB1OT_4 = x.FKUB1OT_4,
                }).ToList();
                using (var DbTPlus = new DbTPlus())
                {
                    foreach (var Item in AllLic)
                    {
                        if (Item.FKUBSXVS != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSXVS));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSXV_2 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSXV_2));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSXV_3 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSXV_3));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSXV_4 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSXV_4));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSOT_1 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSOT_1));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSOT_2 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSOT_2));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSOT_3 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSOT_3));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }
                        if (Item.FKUBSOT_4 != 0)
                        {
                            var TypePu = Item.FKUBSXVS.GetTypePu(nameof(Item.FKUBSOT_4));
                            var Count = DbTPlus.IPU_COUNTERS.Where(x => x.TYPE_PU.Contains(TypePu) && x.FULL_LIC == Item.F4ENUMELS && x.CLOSE_ != true).Count();
                            if (Count == 0)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете нет {TypePu}"));
                            if (Count > 1)
                                ipuNotFoundInLick.Add(IpuNotFoundInLickGetModel(Item.F4ENUMELS, TypePu, $"На лицевом счете более одного {TypePu}"));
                        }

                    }
                }
            }
            cacheApp.Update(User, "Формирую Excel");
            foreach (var Items in ipuNotFoundInLick)
            {
                dt.Rows.Add(Items.Lic, Items.Href, Items.Description, Items.Description);
            }
            cacheApp.Update(User, "Скачиваю Excel");

            return dt;
        }
        private IpuNotFoundInLick IpuNotFoundInLickGetModel(string Lic,string TypePu, string Description)
        {
            return new IpuNotFoundInLick
            {
                Lic = Lic,
                TypePu = TypePu,
                Description = Description,
                Href = $"{ConfigurationManager.AppSettings["App:Host"]}/Counter/DetailedInformIPU?FULL_LIC=={Lic}"
            };
        }
    }
}
