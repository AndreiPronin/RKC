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
using System.Threading;
using DocumentFormat.OpenXml.Drawing.Charts;
using DB.Extention;
using AutoMapper;
using BL.Services;
using DB.DataBase.PaymentV2;

namespace BL.Service
{
    public interface IIntegrations
    {
        Task LoadReadings(string User, ICacheApp cacheApp, DateTime period, INotificationMail _notificationMail, ICounter _counter, string Lic = "", DateTime? paymentDate = null, CancellationToken cancellationToken = default);
        List<IntegrationReadings> GetErrorIntegrationReadings();
        List<IntegrationReadings> GetErrorIntegrationReadings(string FullLic);
    }
    public class Integrations : IIntegrations
    {
        private readonly IMapper _mapper;
        private readonly IMkdInformationService _mkdInformationService;
        public Integrations(IMapper mapper, IMkdInformationService mkdInformationService) {
            _mapper = mapper;
            _mkdInformationService = mkdInformationService;
        }
        public async Task LoadReadings(string User, ICacheApp cacheApp,DateTime period, INotificationMail _notificationMail, ICounter _counter, string Lic = "", DateTime? paymentDate = null, CancellationToken cancellationToken = default)
        {
            object Lock = new object();
            cacheApp.AddProgress(User + "_", "0");
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons(), _mapper, _mkdInformationService);
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var dbs = new DbPaymentV2();
            var DbLIC = new DbLIC();
            var DbTPlus = new DbTPlus();
            var dbApp = new ApplicationDbContext();

            var Counters = await _counter.DetailInfromsAllAsync();

            IQueryable<ALL_LICS> aLL_LICs = DbLIC.ALL_LICS; 
            try
            {
                var periods = period.AddDays(-1);
                IQueryable<IntegrationReadings> Integrs = dbApp.IntegrationReadings.Where(x => x.DateTime >= periods);
                var IntegrsList = await Integrs.ToListAsync();//--------------------------
                var payment = await dbs.Payments.AsNoTracking()
                    .Include(x => x.Counters)
                    .Include(x => x.Banks)
                    .Where(x => x.PaymentDateDay.Value == period)
                    .ToListAsync();
                if (!string.IsNullOrEmpty(Lic))
                {
                    payment = await dbs.Payments.AsNoTracking()
                    .Include(x => x.Counters)
                    .Include(x => x.Banks)
                    .Where(x => x.PaymentDate.Value == paymentDate.Value)
                    .ToListAsync();
                    payment = payment.Where(x=>x.Lic == Lic).ToList();
                }
                var Count = payment.Count();
                int i = 0;
                foreach (var data in payment)
                {

                    var Procent = Math.Round((float)i / Count * 100, 0);
                    var Readings = await aLL_LICs.Select(x => new {
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
                        ZAK = x.ZAK,
                    }).Where(x => x.F4ENUMELS == data.Lic).FirstOrDefaultAsync();

                    cacheApp.UpdateProgress(User, Procent.ToString());
                    i++;
                    if (Readings != null)
                    {
                        foreach (var Item in data.Counters)
                        {
                            var Integr = IntegrsList.Where(x => x.Lic == data.Lic && x.TypePu.Contains(Item.Name) && x.IdCounterReadings == Item.Id).Select(x => x.Lic).ToList();
                            if (Integr.Count() == 0 || !string.IsNullOrEmpty(Lic))
                            {
                                bool Error = false;
                                try
                                {
                                    int? MinValue = 0;
                                    int? MaxValue = 30;
                                    var IPU_COUNTERS = Counters.Where(x => x.FULL_LIC == data.Lic &&
                                    x.TYPE_PU.Contains(Item.Name) && (x.CLOSE_ == null || x.CLOSE_ == false)).ToList();

                                    if (IPU_COUNTERS.Count() != 0 && IPU_COUNTERS.FirstOrDefault().DIMENSION != null)
                                    {
                                        MinValue = IPU_COUNTERS.FirstOrDefault().DIMENSION.MinValue ?? MinValue;
                                        MaxValue = IPU_COUNTERS.FirstOrDefault().DIMENSION.MaxValue ?? MaxValue;
                                    }
                                    var saveModel = new SaveModelIPU();
                                    var integrationReadings = new IntegrationReadings();
                                    integrationReadings.Lic = data.Lic;
                                    integrationReadings.TypePu = Item.Name;
                                    integrationReadings.DateTime = data.PaymentDate;
                                    integrationReadings.IdCounterReadings = Item.Id;
                                    integrationReadings.NowReadings = Item.Value.ToString();
                                    Error = IPU_COUNTERS == null ? true : false;
                                    if (Readings.ZAK != null)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.LicClose.GetDescription()} ";
                                    }
                                    if (IPU_COUNTERS.Count() == 0)
                                    {
                                        Error = true;
                                        var Ipu_Close = Counters.Where(x => x.FULL_LIC == data.Lic &&
                                   x.TYPE_PU.Contains(Item.Name)).Select(x => new { Close = x.CLOSE_ }).FirstOrDefault();
                                        if (Ipu_Close != null)
                                        {
                                            if (Ipu_Close.Close == true)
                                                integrationReadings.Description += $@"{ErrorIntegration.IpuClose.GetDescription()} {Item.Name} ";
                                            else
                                                integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.Name} ";
                                        }
                                        else
                                            integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {Item.Name} ";
                                    }
                                    var DateCheckNext = Counters.Where(x => x.FULL_LIC == data.Lic &&
                                   x.TYPE_PU.Contains(Item.Name) && (x.CLOSE_ == null || x.CLOSE_ == false))
                                        .Select(x => x.DATE_CHECK_NEXT).FirstOrDefault();
                                    if (DateCheckNext.HasValue && Item.Payments.PaymentDateDay >= DateCheckNext.Value.GetDateWhitMaxDate())
                                    {
                                        integrationReadings.Description += $@"{ErrorIntegration.DateCheckNext.GetDescription()} {Item.Name}, 
дата поверки {DateCheckNext.Value.ToString("dd-MM-yyyy")}, дата платежа {Item.Payments.PaymentDateDay.Value.ToString("dd-MM-yyyy")}";
                                        Error = true;
                                    }
                                    if (IPU_COUNTERS.Count() > 1)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                    }
                                    if(IPU_COUNTERS.FirstOrDefault() != null && IPU_COUNTERS.FirstOrDefault().IpuArchiveReasonId == 13)
                                    {
                                        Error = true;
                                        integrationReadings.Description += $@"{ErrorIntegration.NoIndications.GetDescription()} ";
                                    }

                                    saveModel.TypePU = Item.Name;
                                    saveModel.FULL_LIC = data.Lic;
                                    if (Error != true)
                                    {
                                        if (saveModel.TypePU == TypePU.GVS1.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2XVS.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1XVS.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XVS > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XVS < MinValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                            }
                                            else
                                            {
                                                saveModel.FKUB2XVS = Convert.ToDecimal(Item.Value);
                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.GVS2.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2XV_2.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1XV_2.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_2 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_2 < MinValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                            }
                                            else
                                            {
                                                saveModel.FKUB2XV_2 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.GVS3.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2XV_3.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1XV_3.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_3 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_3 < MinValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                            }
                                            else
                                            {
                                                saveModel.FKUB2XV_3 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.GVS4.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2XV_4.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1XV_4.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_4 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2XV_4 < MinValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                            }
                                            else
                                            {
                                                saveModel.FKUB2XV_4 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.ITP1.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2OT_1.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1OT_1.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_1 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_1 <= MinValue)
                                            {
                                                if (Readings.FKUB2OT_1 == 0 && Convert.ToDecimal(Item.Value) == MaxValue)
                                                {
                                                    saveModel.FKUB2OT_1 = Convert.ToDecimal(Item.Value);
                                                }
                                                else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_1 < MinValue)
                                                {
                                                    Error = true;
                                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                                }
                                            }
                                            if (Error == false)
                                            {
                                                saveModel.FKUB2OT_1 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.ITP2.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2OT_2.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1OT_2.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_2 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_2 <= MinValue)
                                            {
                                                if (Readings.FKUB2OT_2 == 0 && Convert.ToDecimal(Item.Value) == MinValue)
                                                {
                                                    saveModel.FKUB2OT_2 = Convert.ToDecimal(Item.Value);
                                                }
                                                else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_2 < MinValue)
                                                {
                                                    Error = true;
                                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                                }
                                            }
                                            if (Error == false)
                                            {
                                                saveModel.FKUB2OT_2 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.ITP3.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2OT_3.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1OT_3.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_3 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_3 <= MinValue)
                                            {
                                                if (Readings.FKUB2OT_3 == 0 && Convert.ToDecimal(Item.Value) == MinValue)
                                                {
                                                    saveModel.FKUB2OT_3 = Convert.ToDecimal(Item.Value);
                                                }
                                                else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_3 < MinValue)
                                                {
                                                    Error = true;
                                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                                }
                                            }
                                            if (Error == false)
                                            {
                                                saveModel.FKUB2OT_3 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                        if (saveModel.TypePU == TypePU.ITP4.GetDescription())
                                        {
                                            integrationReadings.EndReadings = Readings.FKUB2OT_4.ToString();
                                            integrationReadings.InitialReadings = Readings.FKUB1OT_4.ToString();
                                            integrationReadings.NowReadings = Item.Value.ToString();
                                            if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_4 > MaxValue)
                                            {
                                                Error = true;
                                                integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription(MaxValue)} ";
                                            }
                                            else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_4 <= MinValue)
                                            {
                                                if (Readings.FKUB2OT_4 == 0 && Convert.ToDecimal(Item.Value) == MinValue)
                                                {
                                                    saveModel.FKUB2OT_4 = Convert.ToDecimal(Item.Value);
                                                }
                                                else if (Convert.ToDecimal(Item.Value) - Readings.FKUB2OT_4 < MinValue)
                                                {
                                                    Error = true;
                                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                                }
                                            }
                                            if (Error == false)
                                            {
                                                saveModel.FKUB2OT_4 = Convert.ToDecimal(Item.Value);

                                            }
                                        }
                                    }
                                    saveModel.DateTimeIntegraton = data.PaymentDate.Value;
                                    if (Error == false) await counter.UpdatePUIntegrations(saveModel,
                                    "Показания от " + data.Orgs.Name + " дата платежа " + data.PaymentDateDay.Value.ToString(),
                                    IPU_COUNTERS.FirstOrDefault().ID_PU);
                                    else integrationReadings.IsError = Error;

                                    if(string.IsNullOrEmpty(Lic))
                                        dbApp.IntegrationReadings.Add(integrationReadings);
                                    else
                                    {
                                       var integerationList = dbApp.IntegrationReadings.FirstOrDefault(x => x.IdCounterReadings == Item.Id);
                                        if (integerationList == null)
                                            dbApp.IntegrationReadings.Add(integrationReadings);
                                        else
                                        {
                                            integerationList.IsError = Error;
                                            integerationList.Description = integrationReadings.Description;
                                            integerationList.EndReadings = integrationReadings.EndReadings;
                                            integerationList.NowReadings = integrationReadings.NowReadings;
                                            integerationList.InitialReadings = integrationReadings.InitialReadings;

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _notificationMail.Error(ex);
                                    IntegrationReadings integrationReadings = new IntegrationReadings();
                                    integrationReadings.Lic = data.Lic;
                                    integrationReadings.TypePu = Item.Name;
                                    integrationReadings.DateTime = data.PaymentDate;
                                    integrationReadings.IsError = true;
                                    integrationReadings.NowReadings = Item.Value.ToString();
                                    integrationReadings.Description = "Ошибка при интеграции";
                                    if (string.IsNullOrEmpty(Lic))
                                        dbApp.IntegrationReadings.Add(integrationReadings);
                                    else
                                    {
                                        try
                                        {
                                            var integerationList = IntegrsList.FirstOrDefault(x => x.Lic == data.Lic && x.TypePu.Contains(Item.Name) && x.IdCounterReadings == Item.Id);
                                            if (integerationList == null)
                                                dbApp.IntegrationReadings.Add(integrationReadings);
                                            else
                                            {
                                                integerationList.IsError = integrationReadings.IsError;
                                                integerationList.Description = integrationReadings.Description;
                                            }
                                        }
                                        catch
                                        {
                                            dbApp.IntegrationReadings.Add(integrationReadings);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var Integr = dbApp.IntegrationReadings.Where(x => x.Lic == data.Lic && x.DateTime == data.PaymentDateDay).ToList();
                            if (Integr.Count() == 0)
                            {
                                IntegrationReadings integrationReadings = new IntegrationReadings();
                                integrationReadings.Lic = data.Lic;
                                integrationReadings.DateTime = data.PaymentDateDay;
                                if (data.Lic != null)
                                {
                                    if (data.Lic.StartsWith("8") || data.Lic.StartsWith("1")) integrationReadings.IsError = false;
                                    else integrationReadings.IsError = true;
                                }
                                else
                                {
                                    integrationReadings.IsError = true;
                                }
                                integrationReadings.Description = ErrorIntegration.NoLic.GetDescription();
                                if (string.IsNullOrEmpty(Lic))
                                    dbApp.IntegrationReadings.Add(integrationReadings);
                                else
                                {
                                    try
                                    {
                                        var integerationList = IntegrsList.FirstOrDefault(x => x.Lic == data.Lic && x.DateTime == data.PaymentDateDay);
                                        if (integerationList == null)
                                            dbApp.IntegrationReadings.Add(integrationReadings);
                                        else
                                        {
                                            integerationList.IsError = integrationReadings.IsError;
                                            integerationList.Description = integrationReadings.Description;
                                        }
                                    }
                                    catch
                                    {
                                        dbApp.IntegrationReadings.Add(integrationReadings);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _notificationMail.Error(ex);
                        }
                    }
                }
                dbApp.SaveChanges();
                if (string.IsNullOrEmpty(Lic))
                {
                    var LastIntegration = dbApp.Flags.Find(((int)EnumFlags.LastIntegration));
                    LastIntegration.DateTime = period;
                }
                cacheApp.UpdateProgress(User, "100");
                dbApp.SaveChanges();
                dbApp.Dispose();
                dbs.Dispose();
                DbLIC.Dispose();
                DbTPlus.Dispose();
                GC.Collect();
            }
            catch(Exception ex)
            {
                _notificationMail.Error(ex);
            }
        }
        public List<IntegrationReadings> GetErrorIntegrationReadings()
        {
            using (var db = new ApplicationDbContext())
            {
                var result = db.IntegrationReadings.Filter().ToList();
                return result;
            }
        }
        public List<IntegrationReadings> GetErrorIntegrationReadings(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.IntegrationReadings.Filter();
                return query.Where(x => x.Lic == FullLic).ToList();
            }
        }
    }
}
