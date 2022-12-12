using AppCache;
using BE.Counter;
using BL.Counters;
using BL.Helper;
using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using DB.Model;
using BE.Service;
using BL.Extention;
using BL.Notification;

namespace BL.Service
{
    public interface IIntegrations
    {
        Task LoadReadings(string User, ICacheApp cacheApp, DateTime period, INotificationMail _notificationMail);
        List<IntegrationReadings> GetErrorIntegrationReadings();
        List<IntegrationReadings> GetErrorIntegrationReadings(string FullLic);
    }
    public class Integrations : IIntegrations
    {
        public async Task LoadReadings(string User, ICacheApp cacheApp,DateTime period, INotificationMail _notificationMail)
        {
            cacheApp.AddProgress(User, "0");
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons());
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var dbs = new DbPayment();
            var DbLIC = new DbLIC();
            var DbTPlus = new DbTPlus();
            var dbApp = new ApplicationDbContext();
            IQueryable<IPU_COUNTERS> Counter = DbTPlus.IPU_COUNTERS;
            var Counters = Counter.ToList();
            IQueryable<ALL_LICS> aLL_LICs = DbLIC.ALL_LICS;
            var Reading = aLL_LICs.Select(x => new {
                F4ENUMELS = x.F4ENUMELS,
                FKUB2XVS = x.FKUB2XVS,
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
            var periods = period.AddDays(-1);
            IQueryable<IntegrationReadings> Integrs = dbApp.IntegrationReadings.Where(x=>x.DateTime >= periods);
            var IntegrsList = Integrs.ToList();

            var payment = dbs.Payment
                .Include(x => x.Counter)
                .Include(x => x.Organization)
                .Where(x => x.payment_date_day.Value == period)
                .ToList();
            var Count = payment.Count();
            int i = 0;
            foreach(var data in payment)
            {
                var Procent = Math.Round((float)i / Count * 100, 0);
                var Readings = Reading.Where(x => x.F4ENUMELS == data.lic).FirstOrDefault();
                cacheApp.UpdateProgress(User, Procent.ToString());
                i++;
                if (Readings != null)
                {
                    foreach (var Item in data.Counter)
                    {
                        var Integr = IntegrsList.Where(x => x.Lic == data.lic && x.TypePu.Contains(Item.name) && x.IdCounterReadings == Item.id).Select(x => x.Lic).ToList();
                        if (Integr.Count() == 0 && Convert.ToDecimal(Item.value) != 0)
                        {
                            bool Error = false;
                            try
                            {

                                var IPU_COUNTERS = Counters.Where(x => x.FULL_LIC == data.lic &&
                                x.TYPE_PU.Contains(Item.name) && (x.CLOSE_ == null || x.CLOSE_ == false)).Select(x => new { ID_PU = x.ID_PU }).ToList();
                                var saveModel = new SaveModelIPU();
                                var integrationReadings = new IntegrationReadings();
                                integrationReadings.Lic = data.lic;
                                integrationReadings.TypePu = Item.name;
                                integrationReadings.DateTime = data.payment_date;
                                integrationReadings.IdCounterReadings = Item.id;
                                Error = IPU_COUNTERS == null ? true : false;
                                saveModel.TypePU = Item.name;
                                saveModel.FULL_LIC = data.lic;
                                if (saveModel.TypePU == TypePU.GVS1.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2XVS.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1XVS.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2XVS > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2XVS < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2XVS = Convert.ToDecimal(Item.value);
                                    }
                                }
                                if (saveModel.TypePU == TypePU.GVS2.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2XV_2.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1XV_2.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_2 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_2 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2XV_2 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.GVS3.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2XV_3.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1XV_3.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_3 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_3 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2XV_3 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.GVS4.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2XV_4.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1XV_4.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_4 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2XV_4 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2XV_4 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.ITP1.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2OT_1.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1OT_1.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_1 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_1 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2OT_1 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.ITP2.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2OT_2.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1OT_2.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_2 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_2 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2OT_2 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.ITP3.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2OT_3.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1OT_3.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_3 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_3 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2OT_3 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (saveModel.TypePU == TypePU.ITP4.GetDescription())
                                {
                                    integrationReadings.EndReadings = Readings.FKUB2OT_4.ToString();
                                    integrationReadings.InitialReadings = Readings.FKUB1OT_4.ToString();
                                    integrationReadings.NowReadings = Item.value.ToString();
                                    if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_4 > 30)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                    }
                                    else if (Convert.ToDecimal(Item.value) - Readings.FKUB2OT_4 < 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                    }
                                    else if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.name} ";
                                    }
                                    else if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    else
                                    {
                                        saveModel.FKUB2OT_4 = Convert.ToDecimal(Item.value);

                                    }
                                }
                                if (Error == false) await counter.UpdatePUIntegrations(saveModel,
                                    "Показания от " + data.Organization.name + " дата платежа " + data.payment_date_day.Value.ToString(),
                                    IPU_COUNTERS.FirstOrDefault().ID_PU);
                                else integrationReadings.IsError = Error;
                                dbApp.IntegrationReadings.Add(integrationReadings);
                                dbApp.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                _notificationMail.Error(ex);
                                IntegrationReadings integrationReadings = new IntegrationReadings();
                                integrationReadings.Lic = data.lic;
                                integrationReadings.TypePu = Item.name;
                                integrationReadings.DateTime = data.payment_date;
                                integrationReadings.IsError = true;
                                integrationReadings.NowReadings = Item.value.ToString();
                                integrationReadings.Description = "Ошибка при интеграции";
                                dbApp.IntegrationReadings.Add(integrationReadings);

                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        var Integr = dbApp.IntegrationReadings.Where(x => x.Lic == data.lic && x.DateTime == data.payment_date_day).ToList();
                        if (Integr.Count() == 0)
                        {
                            IntegrationReadings integrationReadings = new IntegrationReadings();
                            integrationReadings.Lic = data.lic;
                            integrationReadings.DateTime = data.payment_date_day;
                            if(data.lic != null)
                            {
                                if (data.lic.StartsWith("8") || data.lic.StartsWith("1")) integrationReadings.IsError = false;
                                else integrationReadings.IsError = true;
                            }
                            else
                            {
                                integrationReadings.IsError = true;
                            }
                            integrationReadings.Description = ErrorIntegration.NoLic.GetDescription();
                            dbApp.IntegrationReadings.Add(integrationReadings);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            dbApp.SaveChanges();
            var LastIntegration = dbApp.Flags.Find(((int)EnumFlags.LastIntegration));
            LastIntegration.DateTime = period;
            cacheApp.UpdateProgress(User, "100");
            dbApp.SaveChanges();
            dbApp.Dispose();
            dbs.Dispose();
            DbLIC.Dispose();
            DbTPlus.Dispose();
        }
        public List<IntegrationReadings> GetErrorIntegrationReadings()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.IntegrationReadings.Where(x=>x.IsError == true).ToList();
            }
        }
        public List<IntegrationReadings> GetErrorIntegrationReadings(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.IntegrationReadings.Where(x => x.IsError == true && x.Lic == FullLic).ToList();
            }
        }
    }
}
