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
using System.Threading;
using BL.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using DB.Extention;
using System.Security.Policy;

namespace BL.Counters
{
    public interface ICounter : IBaseService
    {
        List<ALL_LICS> SearchIPU_LIC(SearchIPU_LICModel searchModel);
        List<IPU_COUNTERS> DetailInfroms(string IPU_LIC, bool Close = false);
        Task<List<IPU_COUNTERS>> DetailInfromsAllAsync();
        ALL_LICS Indications(string IPU_LIC);
        List<ALL_LICS_ARCHIVE> HistoryIndinikation(string IPU_LIC);
        List<Log> HistoryEdit(int IdIPU);
        void AutoAddPU(string Full_LIC);
        Task UpdateReadings(SaveModelIPU saveModelIPU);
        void DeleteIPU(int IdPU);
        void AddPU(ModelAddPU modelAddPU, string FIO);
        void DeleteError(int IdPU, string Lic, string User);
        void RecoveryIPU(int IdPU);
        Task UpdatePUIntegrations(SaveModelIPU saveModelIPU, string User, int ID_PU);
        List<IPU_COUNTERS> GetTypeNowUsePU(string FullLIC);
        IEnumerable<ConnectPuWithGisResponse> UpdateGuidPuWithGis(IEnumerable<ConnectPuWithGis> connectPuWithGis);
        IPU_COUNTERS GetInfoPU(string FULL_LIC, string TYPE_PU, bool Close = false);
    }
    public class Counter :BaseService, ICounter
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
                try
                {
                    IQueryable<ALL_LICS> query = model.ALL_LICS;
                    if (searchModel.LIC != 0)
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
                }catch(Exception e)
                {
                    return null;
                }
            }
        }
        public List<IPU_COUNTERS> DetailInfroms(string IPU_LIC,bool Close = false)
        {
            ALL_LICS aLL_LICS = new ALL_LICS();
            using (var DBAll = new DbLIC())
            {
                aLL_LICS = DBAll.ALL_LICS.Where(x => x.F4ENUMELS == IPU_LIC).FirstOrDefault();
            }
            using (var DbTPlus = new DbTPlus())
            {
                IEnumerable<IPU_COUNTERS> iPU_COUNTERs = Enumerable.Empty<IPU_COUNTERS>();
                var DictionatyBrand = DbTPlus.BRAND.Include(x=>x.MODEL).ToList();
                if (Close == false)
                    iPU_COUNTERs = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == IPU_LIC && x.CLOSE_ != true).ToList();
                else
                    iPU_COUNTERs = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == IPU_LIC && x.CLOSE_ == true).ToList();
                foreach (var Items in iPU_COUNTERs)
                {
                    Items.ALL_LICS = aLL_LICS;
                    Items.DIMENSION = DbTPlus.DIMENSIONs.Find(Items.DIMENSION_ID);
                    Items.BrandDictionary = DictionatyBrand;
                }
                return iPU_COUNTERs.OrderBy(x => x.TYPE_PU).ToList();
            }
        }
        public async Task<List<IPU_COUNTERS>> DetailInfromsAllAsync()
        {
           
            using (var DbTPlus = new DbTPlus())
            {
                IEnumerable<IPU_COUNTERS> iPU_COUNTERs = Enumerable.Empty<IPU_COUNTERS>();
                iPU_COUNTERs = await DbTPlus.IPU_COUNTERS.ToListAsync();
                var DIMENSIONs = await DbTPlus.DIMENSIONs.ToListAsync();
                foreach (var Items in iPU_COUNTERs)
                {
                    if(Items.DIMENSION_ID != null)
                        Items.DIMENSION = DIMENSIONs.FirstOrDefault(x => x.ID == Items.DIMENSION_ID.Value);
                }
                return iPU_COUNTERs.OrderBy(x => x.TYPE_PU).ToList();
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
                return DbLIC.ALL_LICS_ARCHIVE.Where(x => x.F4ENUMELS == IPU_LIC).OrderByDescending(x => x.period).ToList();
            }
        }
        public async Task UpdateReadings(SaveModelIPU saveModelIPU)
        {
            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.ID_PU == saveModelIPU.IdPU).FirstOrDefault();
                IPU_COUNTERS.FACTORY_NUMBER_PU = string.IsNullOrEmpty(saveModelIPU.NumberPU) ? IPU_COUNTERS.FACTORY_NUMBER_PU : saveModelIPU.NumberPU;
                IPU_COUNTERS.DATE_CHECK = saveModelIPU.DATE_CHECK == null ? IPU_COUNTERS.DATE_CHECK : saveModelIPU.DATE_CHECK;
                IPU_COUNTERS.CHECKPOINT_DATE = saveModelIPU.CHECKPOINT_DATE == null ? IPU_COUNTERS.CHECKPOINT_DATE : saveModelIPU.CHECKPOINT_DATE;
                IPU_COUNTERS.CHECKPOINT_READINGS = saveModelIPU.CHECKPOINT_READINGS == null ? IPU_COUNTERS.CHECKPOINT_READINGS : saveModelIPU.CHECKPOINT_READINGS;
                IPU_COUNTERS.DATE_CHECK_NEXT = saveModelIPU.DATE_CHECK_NEXT == null ? IPU_COUNTERS.DATE_CHECK_NEXT : saveModelIPU.DATE_CHECK_NEXT;
                IPU_COUNTERS.OPERATOR_CLOSE_READINGS = saveModelIPU.OPERATOR_CLOSE_READINGS == null ? IPU_COUNTERS.OPERATOR_CLOSE_READINGS : saveModelIPU.OPERATOR_CLOSE_READINGS;
                IPU_COUNTERS.OPERATOR_CLOSE_DATE = saveModelIPU.OPERATOR_CLOSE_DATE == null ? IPU_COUNTERS.OPERATOR_CLOSE_DATE : saveModelIPU.OPERATOR_CLOSE_DATE;
                IPU_COUNTERS.MODEL_PU = string.IsNullOrEmpty(saveModelIPU.MODEL_PU) ? IPU_COUNTERS.MODEL_PU : saveModelIPU.MODEL_PU;
                IPU_COUNTERS.TYPEOFSEAL = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL) ? IPU_COUNTERS.TYPEOFSEAL : saveModelIPU.TYPEOFSEAL;
                IPU_COUNTERS.INSTALLATIONDATE = saveModelIPU.INSTALLATIONDATE == null ? IPU_COUNTERS.INSTALLATIONDATE : saveModelIPU.INSTALLATIONDATE;
                IPU_COUNTERS.SEALNUMBER = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER) ? IPU_COUNTERS.SEALNUMBER : saveModelIPU.SEALNUMBER;
                IPU_COUNTERS.DESCRIPTION = string.IsNullOrEmpty(saveModelIPU.DESCRIPTION) ? IPU_COUNTERS.DESCRIPTION : saveModelIPU.DESCRIPTION;
                IPU_COUNTERS.SEALNUMBER2 = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER2) ? IPU_COUNTERS.SEALNUMBER2 : saveModelIPU.SEALNUMBER2;
                IPU_COUNTERS.TYPEOFSEAL2 = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL2) ? IPU_COUNTERS.TYPEOFSEAL2 : saveModelIPU.TYPEOFSEAL2;
                IPU_COUNTERS.GIS_ID_PU = string.IsNullOrEmpty(saveModelIPU.GIS_ID_PU) ? IPU_COUNTERS.GIS_ID_PU : saveModelIPU.GIS_ID_PU;
                IPU_COUNTERS.BRAND_PU = string.IsNullOrEmpty(saveModelIPU.BRAND_PU) ? IPU_COUNTERS.BRAND_PU : saveModelIPU.BRAND_PU;
                IPU_COUNTERS.FULL_LIC = saveModelIPU.FULL_LIC == null ? IPU_COUNTERS.FULL_LIC : saveModelIPU.FULL_LIC;
                IPU_COUNTERS.DIMENSION_ID = saveModelIPU.DIMENSION != null && saveModelIPU.DIMENSION.Id != 0 ? saveModelIPU.DIMENSION.Id : IPU_COUNTERS.DIMENSION_ID;
                DbTPlus.SaveChanges();
            }
            await UpdateLicReadings(saveModelIPU);
        }
        public async Task UpdateReadingsAsync(SaveModelIPU saveModelIPU)
        {

            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.ID_PU == saveModelIPU.IdPU).FirstOrDefault();
                IPU_COUNTERS.FACTORY_NUMBER_PU = string.IsNullOrEmpty(saveModelIPU.NumberPU) ? IPU_COUNTERS.FACTORY_NUMBER_PU : saveModelIPU.NumberPU;
                IPU_COUNTERS.DATE_CHECK = saveModelIPU.DATE_CHECK == null ? IPU_COUNTERS.DATE_CHECK : saveModelIPU.DATE_CHECK;
                IPU_COUNTERS.CHECKPOINT_DATE = saveModelIPU.CHECKPOINT_DATE == null ? IPU_COUNTERS.CHECKPOINT_DATE : saveModelIPU.CHECKPOINT_DATE;
                IPU_COUNTERS.CHECKPOINT_READINGS = saveModelIPU.CHECKPOINT_READINGS == null ? IPU_COUNTERS.CHECKPOINT_READINGS : saveModelIPU.CHECKPOINT_READINGS;
                IPU_COUNTERS.DATE_CHECK_NEXT = saveModelIPU.DATE_CHECK_NEXT == null ? IPU_COUNTERS.DATE_CHECK_NEXT : saveModelIPU.DATE_CHECK_NEXT;
                IPU_COUNTERS.MODEL_PU = string.IsNullOrEmpty(saveModelIPU.MODEL_PU) ? IPU_COUNTERS.MODEL_PU : saveModelIPU.MODEL_PU;
                IPU_COUNTERS.TYPEOFSEAL = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL) ? IPU_COUNTERS.TYPEOFSEAL : saveModelIPU.TYPEOFSEAL;
                IPU_COUNTERS.INSTALLATIONDATE = saveModelIPU.INSTALLATIONDATE == null ? IPU_COUNTERS.INSTALLATIONDATE : saveModelIPU.INSTALLATIONDATE;
                IPU_COUNTERS.SEALNUMBER = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER) ? IPU_COUNTERS.SEALNUMBER : saveModelIPU.SEALNUMBER;
                IPU_COUNTERS.DESCRIPTION = string.IsNullOrEmpty(saveModelIPU.DESCRIPTION) ? IPU_COUNTERS.DESCRIPTION : saveModelIPU.DESCRIPTION;
                IPU_COUNTERS.SEALNUMBER2 = string.IsNullOrEmpty(saveModelIPU.SEALNUMBER2) ? IPU_COUNTERS.SEALNUMBER2 : saveModelIPU.SEALNUMBER2;
                IPU_COUNTERS.TYPEOFSEAL2 = string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL2) ? IPU_COUNTERS.TYPEOFSEAL2 : saveModelIPU.TYPEOFSEAL2;
                IPU_COUNTERS.GIS_ID_PU = string.IsNullOrEmpty(saveModelIPU.GIS_ID_PU) ? IPU_COUNTERS.GIS_ID_PU : saveModelIPU.GIS_ID_PU;
                IPU_COUNTERS.BRAND_PU = string.IsNullOrEmpty(saveModelIPU.BRAND_PU) ? IPU_COUNTERS.BRAND_PU : saveModelIPU.BRAND_PU;
                IPU_COUNTERS.FULL_LIC = saveModelIPU.FULL_LIC == null ? IPU_COUNTERS.FULL_LIC : saveModelIPU.FULL_LIC;
                await DbTPlus.SaveChangesAsync();
            }
            await UpdateLicReadings(saveModelIPU);
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
                if (iPU_COUNTERS.TYPE_PU == "ГВС1")
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
        public void RecoveryIPU(int IdPU)
        {
            using (var DbTPlus = new DbTPlus())
            {

                IPU_COUNTERS iPU_COUNTERS = DbTPlus.IPU_COUNTERS.FirstOrDefault(x => x.ID_PU == IdPU && x.CLOSE_ == true);
                iPU_COUNTERS.CLOSE_ = null;
                iPU_COUNTERS.DATE_CLOSE = null;
                iPU_COUNTERS.OPERATOR_CLOSE_DATE = null;
                iPU_COUNTERS.OPERATOR_CLOSE_READINGS = null;
                DbTPlus.SaveChanges();
            }
        }
        public void DeleteError(int IdPU, string Lic,string User)
        {
            using(var db = new DbTPlus())
            {
                var Pu = db.IPU_COUNTERS.Find(IdPU);
                using (var AppDb = new ApplicationDbContext()) { 
                    var Integr = AppDb.IntegrationReadings.Filter().Where(x => x.TypePu == Pu.TYPE_PU && x.Lic == Lic).ToList();
                    if (Integr?.Count() > 0)
                    {
                        string Description = "";
                        foreach (var Item in Integr)
                        {
                            Description += $@"{Item.Description} нач. {Item.InitialReadings} конч. {Item.EndReadings} 
поступившие {Item.NowReadings}" + Environment.NewLine;
                        }
                        logger.ActionUsers(IdPU, "Убрал ошибку " + Description, User);
                        foreach (var Item in Integr)
                        {
                            Item.IsError = false;
                        }
                        AppDb.SaveChanges();
                    }
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
            //if (iPU_COUNTERs.Count() > 0) { return; }
            if (/*aLL_LICS.FKUB1XVS>0 || aLL_LICS.FKUB2XVS > 0 ||*/ aLL_LICS.FKUBSXVS != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ГВС1")).Count() ==0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС1", FULL_LIC = Full_LIC,DESCRIPTION = "Добавлен Автоматически системой"});
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1XV_2 > 0 || aLL_LICS.FKUB2XV_2 > 0 ||*/ aLL_LICS.FKUBSXV_2 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ГВС2")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС2", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1XV_3 > 0 || aLL_LICS.FKUB2XV_3 > 0 ||*/ aLL_LICS.FKUBSXV_3 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ГВС3")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС3", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1XV_4 > 0 || aLL_LICS.FKUB2XV_4 > 0 ||*/ aLL_LICS.FKUBSXV_4 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ГВС4")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ГВС4", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1OT_1 > 0 || aLL_LICS.FKUB2OT_1 > 0 ||*/ aLL_LICS.FKUBSOT_1 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ОТП1")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП1", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1OT_2 > 0 || aLL_LICS.FKUB2OT_2 > 0 ||*/ aLL_LICS.FKUBSOT_2 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ОТП2")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП2", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1OT_3 > 0 || aLL_LICS.FKUB2OT_3 > 0 ||*/ aLL_LICS.FKUBSOT_3 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ОТП3")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП3", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
            if (/*aLL_LICS.FKUB1OT_4 > 0 || aLL_LICS.FKUB2OT_4 > 0 ||*/ aLL_LICS.FKUBSOT_4 != 0 && iPU_COUNTERs.Where(x => x.TYPE_PU.Contains("ОТП4")).Count() == 0)
            {
                DbTPlus.IPU_COUNTERS.Add(new IPU_COUNTERS { TYPE_PU = "ОТП4", FULL_LIC = Full_LIC, DESCRIPTION = "Добавлен Автоматически системой" });
                DbTPlus.SaveChanges();
            }
        }
        public IPU_COUNTERS GetInfoPU(string FULL_LIC, string TYPE_PU,bool Close = false)
        {
            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == FULL_LIC && x.TYPE_PU == TYPE_PU && x.CLOSE_ != true).FirstOrDefault();
                return IPU_COUNTERS;
            }
        }
        public bool UpdatePU(SaveModelIPU saveModelIPU,string User)
        {
            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == saveModelIPU.FULL_LIC && x.TYPE_PU == saveModelIPU.TypePU && x.CLOSE_ != true).FirstOrDefault();
                if (IPU_COUNTERS != null)
                {
                    saveModelIPU.IdPU = IPU_COUNTERS.ID_PU;
                    saveModelIPU.OVERWRITE_SEAL = false;
                    logger.ActionUsers(saveModelIPU.IdPU, _generatorDescriptons.Generate(saveModelIPU), User);
                   
                    Task.Run(async () => await UpdateReadings(saveModelIPU));
                   
                    //new Thread(x=> UpdateReadings(saveModelIPU)).Start();
                    return true;
                }
                else { return false; }
            }
        }
        public bool UpdateNewPU(SaveModelIPU saveModelIPU, string User)
        {
            using (var DbTPlus = new DbTPlus())
            {
                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == saveModelIPU.FULL_LIC && x.TYPE_PU == saveModelIPU.TypePU && x.CLOSE_ != true).FirstOrDefault();
                if (IPU_COUNTERS == null)
                {
                    AutoAddPU(saveModelIPU.FULL_LIC);
                    IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == saveModelIPU.FULL_LIC && x.TYPE_PU == saveModelIPU.TypePU && x.CLOSE_ != true).FirstOrDefault();
                }
                if (IPU_COUNTERS != null)
                {
                    saveModelIPU.IdPU = IPU_COUNTERS.ID_PU;
                    saveModelIPU.OVERWRITE_SEAL = false;
                   
                    //
                    using (var ctx = new QueueSynchronizationContext())
                    {
                        var tasklogger =  logger.ActionUsersAsync(saveModelIPU.IdPU, _generatorDescriptons.Generate(saveModelIPU), User);
                        ctx.WaitFor(tasklogger);
                        var task = UpdateReadings(saveModelIPU);
                        ctx.WaitFor(task);
                    }
                    //_ = Task.Run(() => UpdateReadings(saveModelIPU));
                    //new Thread(x=> UpdateReadings(saveModelIPU)).Start();
                    return true;
                }
                else { return false; }

            }
        }
        public async Task UpdatePUIntegrations(SaveModelIPU saveModelIPU, string User, int ID_PU)
        {
            if (ID_PU != 0) {
                saveModelIPU.IdPU = ID_PU;
                saveModelIPU.OVERWRITE_SEAL = true;
                var Descriptrion = _generatorDescriptons.Generate(saveModelIPU);
                await UpdateReadingsAsync(saveModelIPU);
                await logger.ActionUsersAsync(saveModelIPU.IdPU, Descriptrion, User);
            }
        }
        public List<IPU_COUNTERS> GetTypeNowUsePU(string FullLIC)
        {
            List<IPU_COUNTERS> model = new List<IPU_COUNTERS>();
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.GVS1.GetDescription(), ID_PU = ((int)TypePU.GVS1) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.GVS2.GetDescription(), ID_PU = ((int)TypePU.GVS2) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.GVS3.GetDescription(), ID_PU = ((int)TypePU.GVS3) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.GVS4.GetDescription(), ID_PU = ((int)TypePU.GVS4) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.ITP1.GetDescription(), ID_PU = ((int)TypePU.ITP1) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.ITP2.GetDescription(), ID_PU = ((int)TypePU.ITP2) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.ITP3.GetDescription(), ID_PU = ((int)TypePU.ITP3) });
            model.Add(new IPU_COUNTERS { TYPE_PU = TypePU.ITP4.GetDescription(), ID_PU = ((int)TypePU.ITP4) });
            using (var db = new DbTPlus())
            {
                var Result = db.IPU_COUNTERS.Where(x => x.FULL_LIC == FullLIC && (x.CLOSE_ == null || x.CLOSE_ == false)).ToList();
                foreach(var Items in Result)
                {
                    if (Items.TYPE_PU == model.FirstOrDefault(x => x.TYPE_PU == Items.TYPE_PU).TYPE_PU) 
                        model.Remove(model.FirstOrDefault(x=>x.TYPE_PU == Items.TYPE_PU));
                }
                return model;
            }
        }

        public IEnumerable<ConnectPuWithGisResponse> UpdateGuidPuWithGis(IEnumerable<ConnectPuWithGis> connectPuWithGis)
        {
            var connectPuWithGisResponses = new List<ConnectPuWithGisResponse>();
            using(var context = new DbTPlus()) 
            {
                foreach (var Item in connectPuWithGis)
                {
                    var ConnectPuWithGisResponse = new ConnectPuWithGisResponse();
                    ConnectPuWithGisResponse.MeteringDeviceGISGKHNumber = Item.MeteringDeviceNumber;
                    ConnectPuWithGisResponse.MeteringDeviceNumber = Item.MeteringDeviceNumber;
                    ConnectPuWithGisResponse.MeteringDeviceRootGUID = Item.MeteringDeviceRootGUID;
                    ConnectPuWithGisResponse.MeteringDeviceVersionGUID = Item.MeteringDeviceVersionGUID;
                    var counters = context.IPU_COUNTERS.Where(x => x.CLOSE_ != true && x.FACTORY_NUMBER_PU == Item.MeteringDeviceNumber).ToList();
                    if (counters.Any())
                    {
                        if (counters.Count() > 1)
                        {
                            ConnectPuWithGisResponse.Description = "Найдено более одного ПУ";
                            ConnectPuWithGisResponse.Error = true;
                            connectPuWithGisResponses.Add(ConnectPuWithGisResponse);
                        }
                        else
                        {
                            var counter = counters.FirstOrDefault();
                            counter.MeteringDeviceGISGKHNumber = Item.MeteringDeviceGISGKHNumber;
                            counter.MeteringDeviceRootGUID = Item.MeteringDeviceRootGUID;
                            counter.MeteringDeviceVersionGUID = Item.MeteringDeviceVersionGUID;
                            context.SaveChanges();
                            ConnectPuWithGisResponse.Description = "Информация обновлена";
                            ConnectPuWithGisResponse.Error = false;
                            connectPuWithGisResponses.Add(ConnectPuWithGisResponse);
                        }
                    }
                    else
                    {
                        ConnectPuWithGisResponse.Description = "Не найден ПУ";
                        ConnectPuWithGisResponse.Error = true;
                        connectPuWithGisResponses.Add(ConnectPuWithGisResponse); 
                    }
                }
            }
            return connectPuWithGisResponses;
        }
    }
}
