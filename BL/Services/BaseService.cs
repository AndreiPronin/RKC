using BE.Counter;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <summary>
        /// заркыть лицевой счет
        /// </summary>
        /// <param name="FullLic"></param>
        /// <returns></returns>
        Task ClosePersInLic(string FullLic);
        /// <summary>
        /// Информацию по типу помещения
        /// </summary>
        /// <param name="FullLic">Номер лицевого счета</param>
        /// <returns></returns>
        FlatTypeDto GetFlatTypeLic (string FullLic);
        /// <summary>
        /// Информацию по типу помещения
        /// </summary>
        /// <param name="FullLic">Id типа помещения</param>
        /// <returns></returns>
        FlatTypeDto GetFlatTypeById(string Id);
        /// <summary>
        /// Проверяет есть ли в базе ПУ с таким номером прибора учета
        /// </summary>
        /// <param name="Number">Номер прибора учета</param>
        /// <param name="changeNumberPu">Измеился ли номер пу для корректировки условий проверки</param>
        void CheckDublicatePuNumber(string Number, string TypePu, bool changeNumberPu = false);
        /// <summary>
        /// Проверяет есть ли в базе ПУ с таким номером прибора учета (Работает только при добавлении)
        /// </summary>
        /// <param name="Number">Номер прибора учета</param>
        void CheckDublicateAddPuNumber(string Number, string TypePu);
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
        public async Task UpdateLicReadings( SaveModelIPU saveModelIPU)
        {
            if (saveModelIPU.FKUB2XVS != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2XVS = saveModelIPU.FKUB2XVS == null ? aLL_LICS.FKUB2XVS : saveModelIPU.FKUB2XVS;
                        aLL_LICS.FKUB1XVS = saveModelIPU.FKUB1XVS == null ? aLL_LICS.FKUB1XVS : saveModelIPU.FKUB1XVS;
                        aLL_LICS.FKUBSXVS = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2XV_2 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2XV_2 = saveModelIPU.FKUB2XV_2 == null ? aLL_LICS.FKUB2XV_2 : saveModelIPU.FKUB2XV_2;
                        aLL_LICS.FKUB1XV_2 = saveModelIPU.FKUB1XV_2 == null ? aLL_LICS.FKUB1XV_2 : saveModelIPU.FKUB1XV_2;
                        aLL_LICS.FKUBSXV_2 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2XV_3 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2XV_3 = saveModelIPU.FKUB2XV_3 == null ? aLL_LICS.FKUB2XV_3 : saveModelIPU.FKUB2XV_3;
                        aLL_LICS.FKUB1XV_3 = saveModelIPU.FKUB1XV_3 == null ? aLL_LICS.FKUB1XV_3 : saveModelIPU.FKUB1XV_3;
                        aLL_LICS.FKUBSXV_3 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2XV_4 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2XV_4 = saveModelIPU.FKUB2XV_4 == null ? aLL_LICS.FKUB2XV_4 : saveModelIPU.FKUB2XV_4;
                        aLL_LICS.FKUB1XV_4 = saveModelIPU.FKUB1XV_4 == null ? aLL_LICS.FKUB1XV_4 : saveModelIPU.FKUB1XV_4;
                        aLL_LICS.FKUBSXV_4 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_1 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_1 = saveModelIPU.FKUB2OT_1 == null ? aLL_LICS.FKUB2OT_1 : saveModelIPU.FKUB2OT_1;
                        aLL_LICS.FKUB1OT_1 = saveModelIPU.FKUB1OT_1 == null ? aLL_LICS.FKUB1OT_1 : saveModelIPU.FKUB1OT_1;
                        aLL_LICS.FKUBSOT_1 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_2 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_2 = saveModelIPU.FKUB2OT_2 == null ? aLL_LICS.FKUB2OT_2 : saveModelIPU.FKUB2OT_2;
                        aLL_LICS.FKUB1OT_2 = saveModelIPU.FKUB1OT_2 == null ? aLL_LICS.FKUB1OT_2 : saveModelIPU.FKUB1OT_2;
                        aLL_LICS.FKUBSOT_2 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_3 != null)
            {

                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_3 = saveModelIPU.FKUB2OT_3 == null ? aLL_LICS.FKUB2OT_3 : saveModelIPU.FKUB2OT_3;
                        aLL_LICS.FKUB1OT_3 = saveModelIPU.FKUB1OT_3 == null ? aLL_LICS.FKUB1OT_3 : saveModelIPU.FKUB1OT_3;
                        aLL_LICS.FKUBSOT_3 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
            if (saveModelIPU.FKUB2OT_4 != null)
            {
                using (var DbLIC = new DbLIC())
                {
                    ALL_LICS aLL_LICS = await DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefaultAsync();
                    if (saveModelIPU.OVERWRITE_SEAL)
                    {
                        aLL_LICS.FKUB2OT_4 = saveModelIPU.FKUB2OT_4 == null ? aLL_LICS.FKUB2OT_4 : saveModelIPU.FKUB2OT_4;
                        aLL_LICS.FKUB1OT_4 = saveModelIPU.FKUB1OT_4 == null ? aLL_LICS.FKUB1OT_4 : saveModelIPU.FKUB1OT_4;
                        aLL_LICS.FKUBSOT_4 = 1;
                        await DbLIC.SaveChangesAsync();
                    }
                }
            }
        }
        public FlatTypeDto GetFlatTypeLic(string FullLic)
        {
            using (var DbLIC = new DbLIC())
            {
                var res = DbLIC.ALL_LICS.Select(x=>new { x.F4EPLOMBA, x.F4ENUMELS }).FirstOrDefault(x => x.F4ENUMELS == FullLic);
                var Id = res.F4EPLOMBA;
                var FlatType = DbLIC.FlatTypes.FirstOrDefault(x => x.FlatTypeId == Id);
                return FlatType == null ? new FlatTypeDto() : FlatType;
            }

        }
        public FlatTypeDto GetFlatTypeById(string Id)
        {
            using (var DbLIC = new DbLIC())
            {
                var FlatType = DbLIC.FlatTypes.FirstOrDefault(x => x.FlatTypeId == Id);
                return FlatType == null ? new FlatTypeDto() : FlatType;
            }

        }
        public void CheckDublicatePuNumber(string Number, string typePu, bool changeNumberPu = false)
        {
            if (string.IsNullOrEmpty(Number))
                return;
            using(var context = new DbTPlus())
            {
                typePu = Regex.Replace(typePu, "([0-9]+)", "");
                var result = context.IPU_COUNTERS.Where(x=>x.FACTORY_NUMBER_PU == Number && x.CLOSE_ != true && x.TYPE_PU.Contains(typePu)).ToList();
                if (changeNumberPu)
                {
                    if (result != null && result.Count() > 1)
                        throw new Exception($@"Такой номер уже существует на лицевом счете {result.FirstOrDefault()?.FULL_LIC}, {result.LastOrDefault()?.FULL_LIC} 
тип прибора учета {result.FirstOrDefault()?.TYPE_PU}, {result.LastOrDefault()?.TYPE_PU}");
                }
            }
        }
        public void CheckDublicateAddPuNumber(string Number, string TypePu)
        {
            if (string.IsNullOrEmpty(Number))
                return;
            using (var context = new DbTPlus())
            {
                TypePu = Regex.Replace(TypePu, "([0-9]+)", "");
                var result = context.IPU_COUNTERS.Where(x => x.FACTORY_NUMBER_PU == Number && x.CLOSE_ != true && x.TYPE_PU.Contains(TypePu)).ToList();
                if (result != null && result.Count() >= 1)
                    throw new Exception($@"Такой номер уже существует на лицевом счете {result.FirstOrDefault()?.FULL_LIC}
тип прибора учета {result.FirstOrDefault()?.TYPE_PU}");
            }
        }
    }
}
