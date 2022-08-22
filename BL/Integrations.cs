using AppCache;
using BE.Counter;
using BL.Counters;
using BL.Helper;
using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DB.Model;

namespace BL
{
    public interface IIntegrations
    {
        void LoadReadings(string User,ICacheApp cacheApp);
        List<IntegrationReadings> GetErrorIntegrationReadings();
    }
    public class Integrations : IIntegrations
    {
        public void LoadReadings(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "0");
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons());
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var DbLIC = new DbLIC();
            DbPayment dbs = new DbPayment();
            ApplicationDbContext dbApp = new ApplicationDbContext();
            var DbTPlus = new DbTPlus();
            var payment = dbs.Payment
                .Include(x => x.Counter)
                .Include(x => x.Organization)
                .ToList();
            var Count = payment.Count();
            bool Error = false;
            int i = 0;
            foreach (var data in payment)
            {
                var Procent = Math.Round((float)i / Count * 100, 0);
                var Readings = DbLIC.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == data.lic);
                cacheApp.UpdateProgress(User, Procent.ToString());
                i++;
                if (Readings != null)
                {
                    foreach (var Item in data.Counter)
                    {
                        var Integr = dbApp.IntegrationReadings.Where(x => x.Lic == data.lic && x.TypePu.Contains(Item.name) && x.DateTime == data.payment_date_day).ToList();
                        if (Integr.Count() == 0)
                        {
                            Error = false;
                            try
                            {
                                var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == data.lic && x.TYPE_PU.Contains(Item.name) && (x.CLOSE_ == null || x.CLOSE_ == false)).ToList();
                                SaveModelIPU saveModel = new SaveModelIPU();
                                IntegrationReadings integrationReadings = new IntegrationReadings();
                                integrationReadings.Lic = data.lic;
                                integrationReadings.TypePu = Item.name;
                                integrationReadings.DateTime = data.payment_date_day;
                                saveModel.TypePU = Item.name;
                                saveModel.FULL_LIC = data.lic;
                                if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description = ErrorIntegration.NoPU.GetDescription();
                                }
                                if (saveModel.TypePU == TypePU.GVS1.GetDescription())
                                {
                                    if (Readings.FKUB2XVS - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2XV_2 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2XV_3 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2XV_4 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2OT_1 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2OT_2 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2OT_3 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                    if (Readings.FKUB2OT_4 - Convert.ToDecimal(Item.value) > 50)
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
                                        integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} ";
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
                                if (Error == false) counter.UpdatePUIntegrations(saveModel, User, IPU_COUNTERS.FirstOrDefault());
                                else integrationReadings.IsError = Error;
                                dbApp.IntegrationReadings.Add(integrationReadings);
                                dbApp.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                IntegrationReadings integrationReadings = new IntegrationReadings();
                                integrationReadings.Lic = data.lic;
                                integrationReadings.TypePu = Item.name;
                                integrationReadings.DateTime = data.payment_date_day;
                                integrationReadings.IsError = true;
                                integrationReadings.Description = "Ошибка при интеграции";
                                dbApp.IntegrationReadings.Add(integrationReadings);
                            }
                        }
                    }
                }
                else
                {
                    IntegrationReadings integrationReadings = new IntegrationReadings();
                    integrationReadings.Lic = data.lic;
                    integrationReadings.DateTime = data.payment_date_day;
                    integrationReadings.IsError = true;
                    integrationReadings.Description = ErrorIntegration.NoLic.GetDescription();
                    dbApp.IntegrationReadings.Add(integrationReadings);
                }
            }
        }
        public List<IntegrationReadings> GetErrorIntegrationReadings()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.IntegrationReadings.ToList();
            }
        }
    }
}
