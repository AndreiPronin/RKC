using BE.Counter;
using DB.DataBase;
using DB.Model;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Helper;

namespace BL.Counters
{
    public interface ICounter
    {
        List<ALL_LICS> SearchIPU_LIC(SearchIPU_LICModel searchModel);
        List<IPU_COUNTERS> DetailInfroms(string IPU_LIC);
        List<IPU_COUNTERS> DetailInfromsDelete(string IPU_LIC);
        ALL_LICS Indications(string IPU_LIC);
        List<ALL_LICS_ARCHIVE> HistoryIndinikation(string IPU_LIC);
        List<Log> HistoryEdit(int IdIPU);
        void AutoAddPU(string Full_LIC);
        void UpdateReadings(SaveModelIPU saveModelIPU);
        void DeleteIPU(int IdPU);
        void AddPU(ModelAddPU modelAddPU, string FIO);
    }
    public class Counter: ICounter
    {
        private readonly Ilogger logger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        public Counter(Ilogger loggers, IGeneratorDescriptons generatorDescriptons)
        {
            logger = loggers;
            _generatorDescriptons = generatorDescriptons;
        }
        public List<ALL_LICS> SearchIPU_LIC(SearchIPU_LICModel searchModel)
        {
            using (var model = new DbLIC())
            {
                IQueryable<ALL_LICS> query = model.ALL_LICS;
                if (searchModel.LIC !=0)
                    query = query.Where(x => x.F4ENUMELS.Contains(searchModel.LIC.ToString()));
                if (!string.IsNullOrEmpty(searchModel.FIO))
                    query = query.Where(x => x.FIO.Contains(searchModel.FIO));
                if (!string.IsNullOrEmpty(searchModel.street))
                    query = query.Where(x => x.UL.Contains(searchModel.street));
                if (!string.IsNullOrEmpty(searchModel.home))
                    query = query.Where(x => x.DOM == searchModel.home);
                if (!string.IsNullOrEmpty(searchModel.flat))
                    query = query.Where(x => x.KW.Contains(searchModel.flat));
                return query.Take(30).ToList();
            }
        }
        public List<IPU_COUNTERS> DetailInfroms(string IPU_LIC)
        {
            ALL_LICS aLL_LICS = new ALL_LICS();
            using (var DBAll = new DbLIC())
            {
                aLL_LICS = DBAll.ALL_LICS.Where(x => x.F4ENUMELS == IPU_LIC).FirstOrDefault();
            }
            using (var DbTPlus = new DbTPlus())
            {
                List<IPU_COUNTERS> iPU_COUNTERs = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == IPU_LIC && x.CLOSE_ != true).ToList();
                foreach (var Items in iPU_COUNTERs)
                {
                    Items.ALL_LICS = aLL_LICS;
                }
                return iPU_COUNTERs.OrderBy(x=>x.TYPE_PU).ToList();
            }
        }
        public List<IPU_COUNTERS> DetailInfromsDelete(string IPU_LIC)
        {
            ALL_LICS aLL_LICS = new ALL_LICS();
            using (var DBAll = new DbLIC())
            {
                aLL_LICS = DBAll.ALL_LICS.Where(x => x.F4ENUMELS == IPU_LIC).FirstOrDefault();
            }
            using (var DbTPlus = new DbTPlus())
            {
                List<IPU_COUNTERS> iPU_COUNTERs = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == IPU_LIC && x.CLOSE_ == true).ToList();
                foreach (var Items in iPU_COUNTERs)
                {
                    Items.ALL_LICS = aLL_LICS;
                }
                return iPU_COUNTERs.OrderBy(x => x.ID_PU).ToList();
            }
        }
        public ALL_LICS Indications(string IPU_LIC)
        {
            using (var DbLIC = new DbLIC())
            {
                return DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == IPU_LIC).FirstOrDefault();
            }
        }
        public List<ALL_LICS_ARCHIVE> HistoryIndinikation(string IPU_LIC)
        {
            using (var DbLIC = new DbLIC())
            {
                return DbLIC.ALL_LICS_ARCHIVE.Where(x => x.F4ENUMELS == IPU_LIC).OrderByDescending(x=>x.period).ToList();
            }
        }
        public void UpdateReadings(SaveModelIPU saveModelIPU)
        {
            
                using (var DbTPlus = new DbTPlus())
                {
                   
                    var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.ID_PU == saveModelIPU.IdPU).FirstOrDefault();
                    IPU_COUNTERS.FACTORY_NUMBER_PU =  string.IsNullOrEmpty(saveModelIPU.NumberPU) ? IPU_COUNTERS.FACTORY_NUMBER_PU : saveModelIPU.NumberPU;
                    IPU_COUNTERS.DATE_CHECK = saveModelIPU.DATE_CHECK == null ? IPU_COUNTERS.DATE_CHECK : saveModelIPU.DATE_CHECK;
                    IPU_COUNTERS.DATE_CHECK_NEXT = saveModelIPU.DATE_CHECK_NEXT == null ? IPU_COUNTERS.DATE_CHECK_NEXT : saveModelIPU.DATE_CHECK_NEXT;
                    IPU_COUNTERS.MODEL_PU = string.IsNullOrEmpty(saveModelIPU.MODEL_PU) ? IPU_COUNTERS.MODEL_PU : saveModelIPU.MODEL_PU;
                    IPU_COUNTERS.TYPEOFSEAL = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL ) ? IPU_COUNTERS.TYPEOFSEAL : saveModelIPU.TYPEOFSEAL;
                    IPU_COUNTERS.INSTALLATIONDATE = saveModelIPU.INSTALLATIONDATE == null ? IPU_COUNTERS.INSTALLATIONDATE : saveModelIPU.INSTALLATIONDATE;
                    IPU_COUNTERS.SEALNUMBER = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER) ? IPU_COUNTERS.SEALNUMBER : saveModelIPU.SEALNUMBER;
                    IPU_COUNTERS.DESCRIPTION = string.IsNullOrEmpty(saveModelIPU.DESCRIPTION) ? IPU_COUNTERS.DESCRIPTION : saveModelIPU.DESCRIPTION;
                IPU_COUNTERS.SEALNUMBER2 = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER2) ? IPU_COUNTERS.SEALNUMBER2 : saveModelIPU.SEALNUMBER2;
                IPU_COUNTERS.TYPEOFSEAL2 = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL2) ? IPU_COUNTERS.TYPEOFSEAL2 : saveModelIPU.TYPEOFSEAL2;
                IPU_COUNTERS.FULL_LIC = saveModelIPU.FULL_LIC == null ? IPU_COUNTERS.FULL_LIC : saveModelIPU.FULL_LIC;
                    DbTPlus.SaveChanges();
                  
                }
            if (saveModelIPU.FKUB2XVS != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if (saveModelIPU.OVERWRITE_SEAL )
                    {
                        try
                        {
                            aLL_LICS.FKUB2XVS = saveModelIPU.FKUB2XVS == null ? aLL_LICS.FKUB2XVS : saveModelIPU.FKUB2XVS;
                            aLL_LICS.FKUB1XVS = saveModelIPU.FKUB1XVS == null ? aLL_LICS.FKUB1XVS : saveModelIPU.FKUB1XVS;
                            aLL_LICS.FKUBSXVS = 1;
                            DbLIC.SaveChanges();
                        }catch(Exception ex) { }
                    }
                }
            }
            if (saveModelIPU.FKUB2XV_2 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if (saveModelIPU.OVERWRITE_SEAL )
                    {
                        aLL_LICS.FKUB2XV_2 = saveModelIPU.FKUB2XV_2 == null ? aLL_LICS.FKUB2XV_2 : saveModelIPU.FKUB2XV_2;
                        aLL_LICS.FKUB1XV_2 = saveModelIPU.FKUB1XV_2 == null ? aLL_LICS.FKUB1XV_2 : saveModelIPU.FKUB1XV_2;
                        aLL_LICS.FKUBSXV_2 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }
            if (saveModelIPU.FKUB2XV_3 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if (saveModelIPU.OVERWRITE_SEAL )
                    {
                        aLL_LICS.FKUB2XV_3 = saveModelIPU.FKUB2XV_3 == null ? aLL_LICS.FKUB2XV_3 : saveModelIPU.FKUB2XV_3;
                        aLL_LICS.FKUB1XV_3 = saveModelIPU.FKUB1XV_3 == null ? aLL_LICS.FKUB1XV_3 : saveModelIPU.FKUB1XV_3;
                        aLL_LICS.FKUBSXV_3 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }

            if (saveModelIPU.FKUB2XV_4 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if ( saveModelIPU.OVERWRITE_SEAL )
                    {
                        aLL_LICS.FKUB2XV_4 = saveModelIPU.FKUB2XV_4 == null ? aLL_LICS.FKUB2XV_4 : saveModelIPU.FKUB2XV_4;
                        aLL_LICS.FKUB1XV_4 = saveModelIPU.FKUB1XV_4 == null ? aLL_LICS.FKUB1XV_4 : saveModelIPU.FKUB1XV_4;
                        aLL_LICS.FKUBSXV_4 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }

            if (saveModelIPU.FKUB2OT_1 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if ( saveModelIPU.OVERWRITE_SEAL )
                    {
                        aLL_LICS.FKUB2OT_1 = saveModelIPU.FKUB2OT_1 == null ? aLL_LICS.FKUB2OT_1 : saveModelIPU.FKUB2OT_1;
                        aLL_LICS.FKUB1OT_1 = saveModelIPU.FKUB1OT_1 == null ? aLL_LICS.FKUB1OT_1 : saveModelIPU.FKUB1OT_1;
                        aLL_LICS.FKUBSOT_1 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }

            if (saveModelIPU.FKUB2OT_2 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_2 = saveModelIPU.FKUB2OT_2 == null ? aLL_LICS.FKUB2OT_2 : saveModelIPU.FKUB2OT_2;
                        aLL_LICS.FKUB1OT_2 = saveModelIPU.FKUB1OT_2 == null ? aLL_LICS.FKUB1OT_2 : saveModelIPU.FKUB1OT_2;
                        aLL_LICS.FKUBSOT_2 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_3 != null)
            {

                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if (saveModelIPU.OVERWRITE_SEAL )
                    {
                        aLL_LICS.FKUB2OT_3 = saveModelIPU.FKUB2OT_3 == null ? aLL_LICS.FKUB2OT_3 : saveModelIPU.FKUB2OT_3;
                        aLL_LICS.FKUB1OT_3 = saveModelIPU.FKUB1OT_3 == null ? aLL_LICS.FKUB1OT_3 : saveModelIPU.FKUB1OT_3;
                        aLL_LICS.FKUBSOT_3 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_4 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                    if ( saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_4 = saveModelIPU.FKUB2OT_4 == null ? aLL_LICS.FKUB2OT_4 : saveModelIPU.FKUB2OT_4;
                        aLL_LICS.FKUB1OT_4 = saveModelIPU.FKUB1OT_4 == null ? aLL_LICS.FKUB1OT_4 : saveModelIPU.FKUB1OT_4;
                        aLL_LICS.FKUBSOT_4 = 1;
                        DbLIC.SaveChanges();
                    }
                }
            }
        }
        public void DeleteIPU(int IdPU)
        {
            using (var DbTPlus = new DbTPlus())
            {
               
                IPU_COUNTERS iPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.ID_PU == IdPU && x.CLOSE_ != true).ToList()?.Last();
                iPU_COUNTERS.CLOSE_ = true;
                iPU_COUNTERS.DATE_CLOSE = DateTime.Now;
                DbTPlus.SaveChanges();
                DbLIC dbLIC = new DbLIC();
                var AllLic = dbLIC.ALL_LICS.Where(x => x.F4ENUMELS == iPU_COUNTERS.FULL_LIC).FirstOrDefault();
                if(iPU_COUNTERS.TYPE_PU == "ГВС1")
                {
                    AllLic.FKUB2XVS = 0;
                    AllLic.FKUB1XVS = 0;
                    AllLic.FKUBSXVS = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ГВС2")
                {
                    AllLic.FKUB2XV_2 = 0;
                    AllLic.FKUB1XV_2 = 0;
                    AllLic.FKUBSXV_2 = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ГВС3")
                {
                    AllLic.FKUB2XV_3 = 0;
                    AllLic.FKUB1XV_3 = 0;
                    AllLic.FKUBSXV_3 = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ГВС4")
                {
                    AllLic.FKUB2XV_4 = 0;
                    AllLic.FKUB1XV_4 = 0;
                    AllLic.FKUBSXV_4 = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ОТП1")
                {
                    AllLic.FKUB2OT_1 = 0;
                    AllLic.FKUB1OT_1 = 0;
                    AllLic.FKUBSOT_1 = 0;
                    dbLIC.SaveChanges();
                    
                }
                if (iPU_COUNTERS.TYPE_PU == "ОТП2")
                {
                    AllLic.FKUB2OT_2 = 0;
                    AllLic.FKUB1OT_2 = 0;
                    AllLic.FKUBSOT_2 = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ОТП3")
                {
                    AllLic.FKUB2OT_3 = 0;
                    AllLic.FKUB1OT_3 = 0;
                    AllLic.FKUBSOT_3 = 0;
                    dbLIC.SaveChanges();
                }
                if (iPU_COUNTERS.TYPE_PU == "ОТП4")
                {
                    AllLic.FKUB2OT_4 = 0;
                    AllLic.FKUB1OT_4 = 0;
                    AllLic.FKUBSOT_4 = 0;
                    dbLIC.SaveChanges();
                }
            }
        }
        public List<Log> HistoryEdit(int IdIPU)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Log.Where(x => x.IdPU == IdIPU).OrderByDescending(x => x.DateTime).ToList();
            }
        }
        public void AddPU(ModelAddPU modelAddPU, string FIO)
        {
            using (var DbTPlus = new DbTPlus())
            {
                IPU_COUNTERS iPU_COUNTERS = ConvertToModel.ModelAddpu_To_IPU_COUNTERS(modelAddPU);
                DbTPlus.IPU_COUNTERS.Add(iPU_COUNTERS);
                DbTPlus.SaveChanges();
                DbTPlus.Entry(iPU_COUNTERS).GetDatabaseValues();
                int id = iPU_COUNTERS.ID_PU;
                DbTPlus.IPU_counters_PE.Add(new IPU_counters_PE
                {
                    FULL_LIC = modelAddPU.FULL_LIC,
                    DATE_CHECK_NEXT = modelAddPU.DATE_CHECK_NEXT,
                    FACTORY_NUMBER_PU = modelAddPU.FACTORY_NUMBER_PU,
                    id_pu = id,
                    TYPE_PU = iPU_COUNTERS.TYPE_PU
                });
                DbTPlus.SaveChanges();
               
                logger.ActionUsers(id, $"Добавили прибор учета к лицевому счету {modelAddPU.FULL_LIC}", FIO);
            }
            using (var DbLIC = new DbLIC())
            {
                ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == modelAddPU.FULL_LIC).FirstOrDefault();
                if (modelAddPU.TYPE_PU == TypePU.GVS1) { aLL_LICS.FKUBSXVS = 1; aLL_LICS.FKUB1XVS = modelAddPU.InitialReadings;aLL_LICS.FKUB2XVS = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.GVS2) { aLL_LICS.FKUBSXV_2 = 1; aLL_LICS.FKUB1XV_2 = modelAddPU.InitialReadings; aLL_LICS.FKUB2XV_2 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.GVS3) { aLL_LICS.FKUBSXV_3 = 1; aLL_LICS.FKUB1XV_3 = modelAddPU.InitialReadings; aLL_LICS.FKUB2XV_3 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.GVS4) { aLL_LICS.FKUBSXV_4 = 1; aLL_LICS.FKUB1XV_4 = modelAddPU.InitialReadings; aLL_LICS.FKUB2XV_4 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.ITP1) { aLL_LICS.FKUBSOT_1 = 1; aLL_LICS.FKUB1OT_1 = modelAddPU.InitialReadings; aLL_LICS.FKUB2OT_1 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.ITP2) { aLL_LICS.FKUBSOT_2 = 1; aLL_LICS.FKUB1OT_2 = modelAddPU.InitialReadings; aLL_LICS.FKUB2OT_2 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.ITP3) { aLL_LICS.FKUBSOT_3 = 1; aLL_LICS.FKUB1OT_3 = modelAddPU.InitialReadings; aLL_LICS.FKUB2OT_3 = modelAddPU.EndReadings; }
                if (modelAddPU.TYPE_PU == TypePU.ITP4) { aLL_LICS.FKUBSOT_4 = 1; aLL_LICS.FKUB1OT_4 = modelAddPU.InitialReadings; aLL_LICS.FKUB2OT_4 = modelAddPU.EndReadings; }
                    DbLIC.SaveChanges();

            }

        }
        public void AutoAddPU(string Full_LIC)
        {
            ALL_LICS aLL_LICS = new ALL_LICS();
            using (var DBAll = new DbLIC())
            {
                aLL_LICS = DBAll.ALL_LICS.Where(x => x.F4ENUMELS == Full_LIC).FirstOrDefault();
            }
            var DbTPlus = new DbTPlus();
            List<IPU_COUNTERS> iPU_COUNTERs = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == Full_LIC).ToList();
            if (iPU_COUNTERs.Count() > 0) { return; }
            if(aLL_LICS.FKUB1XVS>0 || aLL_LICS.FKUB2XVS > 0 || aLL_LICS.FKUBSXVS !=0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС1", FULL_LIC = Full_LIC,DESCRIPTION = "Добавлен Автоматически системой"});
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1XV_2 > 0 || aLL_LICS.FKUB2XV_2 > 0 || aLL_LICS.FKUBSXV_2 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС2", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1XV_3 > 0 || aLL_LICS.FKUB2XV_3 > 0 || aLL_LICS.FKUBSXV_3 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС3", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1XV_4 > 0 || aLL_LICS.FKUB2XV_4 > 0 || aLL_LICS.FKUBSXV_4 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС4", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1OT_1 > 0 || aLL_LICS.FKUB2OT_1 > 0 || aLL_LICS.FKUBSOT_1 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП1", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1OT_2 > 0 || aLL_LICS.FKUB2OT_2 > 0 || aLL_LICS.FKUBSOT_2 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП2", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1OT_3 > 0 || aLL_LICS.FKUB2OT_3 > 0 || aLL_LICS.FKUBSOT_3 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП3", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (aLL_LICS.FKUB1OT_4 > 0 || aLL_LICS.FKUB2OT_4 > 0 || aLL_LICS.FKUBSOT_4 != 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП4", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }


        }
        public bool UpdatePU(SaveModelIPU saveModelIPU,string User)
        {
            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == saveModelIPU.FULL_LIC && x.TYPE_PU == saveModelIPU.TypePU && x.CLOSE_ == null).FirstOrDefault();
                if (IPU_COUNTERS != null)
                {
                    saveModelIPU.IdPU = IPU_COUNTERS.ID_PU;
                    saveModelIPU.OVERWRITE_SEAL = true;
                    logger.ActionUsers(saveModelIPU.IdPU, _generatorDescriptons.Generate(saveModelIPU), User);
                    UpdateReadings(saveModelIPU);
                    return true;
                }
                else { return false; }

            }
        }
    }
}
