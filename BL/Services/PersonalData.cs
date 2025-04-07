using AutoMapper;
using BE.Counter;
using BE.PersData;
using BL.Counters;
using BL.Excel;
using BL.Extention;
using BL.Helper;
using DB.DataBase;
using DB.DataBase.PaymentV2;
using DB.DataBase.PaymentV2Archive;
using DB.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IPersonalData : IBaseService
    {
        List<PersonalInformations> GetPersonalInformation(string FullLic);
        StateCalculation GetStateCalculation(string FullLic);
        List<PersData> GetInfoPersData(string FullLic);
        List<PersData> GetInfoPersDataDelete(string FullLic);
        ALL_LICS GetNoteAllLic(string FullLic);
        string SaveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile, string User);
        PersDataDocumentLoad DownLoadFile(int Id);
        Task<List<HelpCalculationsModel>> GetInfoHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
        Task<PersDataDocumentLoad> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
        string DeleteFile(int Id, string User);
        List<LogsPersData> GetHistory(int idPersData);
        void SavePersonalData(PersDataModel persDataModelView, string User);
        void SavePersonalDataMain(PersDataModel persDataModel, string User);
        void MakeToMain(int idPersData, string User);
        void AddPersData(PersDataModel persDataModel, string User);
        void DeletePers(int IdPersData, string User);
        string GetRoomTypeMain(string Full_Lic);
        List<PaymentHistoryResponse> GetPaymentHistory(string Full_Lic);
        List<ReadingsHistoryResponse> GetReadingsHistory(string Full_Lic);
        void CloseLic(string FullLic, ICounter _counter, string User);
        void OpenLic(string FullLic);
        List<DB.Model.Counters> GetReadingsHistorySearch(string Parametr,string Full_Lic);
        void UpdateSquareFlat(double? Square, string Lic);
        void UpdatePersDataSquareExcel(PersDataModel persDataModel, string User);
        void SavePersonalDataFioLic(PersDataModel persDataModel);
        Task CloseLicAsync(string FullLic, string Description, ICounter _counter, string User);
        DebtInfoForLic GetDebtInfoForLic(string FullLic);
        Task<DebtInfoForLic> GetDebtInfoForLicAsync(string FullLic);
        List<DebtInfoForLic> GetDebtInfosForLics(List<string> lics);
        Task<List<ManualRecalculationsByFullLic>> GetManualRecalculationsByFullLic(string FullLic);
        Task RemoveRecalculation(Guid Id, int serviceId);
        Task<List<ALL_LICS_ARCHIVE>> GetHistoryAccrualsByItems(string fullLic);
    }
    public class PersonalData : BaseService, IPersonalData
    {
        private readonly Ilogger _ilogger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly IDictionary _dictionary;
        private readonly IMapper _mapper;
        public PersonalData(Ilogger ilogger, IGeneratorDescriptons generatorDescriptons, IDictionary dictionary, IMapper mapper)
        {
            _ilogger = ilogger;
            _generatorDescriptons = generatorDescriptons;
            _dictionary = dictionary;
            _mapper = mapper;
        }
        public List<PersonalInformations> GetPersonalInformation(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.PersonalInformation.Where(x => x.full_lic == FullLic).ToList();
            }
        }
        public StateCalculation GetStateCalculation(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    return db.StateCalculation.Where(x => x.F4ENUMELS == FullLic).OrderByDescending(x => x.Period).First();
                }
                catch { return new StateCalculation() { Period = DateTime.Now }; }
            }
        }
        public DebtInfoForLic GetDebtInfoForLic(string FullLic)
        {
            using (var db = new DbLIC())
            {
                var result = db.ALL_LICS.Where(x => x.F4ENUMELS == FullLic).Select(x =>
                new DebtInfoForLic()
                {
                    Lic = x.F4ENUMELS,
                    Debt = Math.Round((double)(x.TDK + x.PENY_TDK), 2),
                    Payment = 0,
                    CurrentDebt = Math.Round((double)(x.TDK + x.PENY_TDK), 2)
                }).FirstOrDefault();

                if (result != null)
                {
                    using (var paymentDb = new DbPaymentV2())
                    {
                        var query = paymentDb.Payments.Where(x => x.Lic == FullLic && x.TransactionAmount != 0);

                        if (query.Count() > 0)
                        {
                            var paymentAmount = query.Sum(x => x.TransactionAmount);
                            var paymentAmountDouble = Math.Round((double)paymentAmount, 2);
                            result.Payment += paymentAmountDouble;
                            result.CurrentDebt -= paymentAmountDouble;
                            result.Payment = Math.Round(result.Payment, 2);
                            result.CurrentDebt = Math.Round(result.CurrentDebt, 2);
                        }
                    }
                }

                return result;
            }
        }
        public List<DebtInfoForLic> GetDebtInfosForLics(List<string> lics)
        {
            using (var db = new DbLIC())
            {
                var result = db.ALL_LICS.Where(x => lics.Contains(x.F4ENUMELS)).Select(x =>
                new DebtInfoForLic()
                {
                    Lic = x.F4ENUMELS,
                    Debt = Math.Round((double)(x.TDK + x.PENY_TDK), 2),
                    Payment = 0,
                    CurrentDebt = Math.Round((double)(x.TDK + x.PENY_TDK), 2)
                }).ToList();

                if (result.Count > 0)
                {
                    using (var paymentDb = new DbPaymentV2())
                    {
                        var paymentsByLic = paymentDb.Payments.Where(x => lics.Contains(x.Lic))
                            .GroupBy(x => x.Lic).ToDictionary(x => x.Key, y => y.Sum(x => x.TransactionAmount));

                        foreach (var debt in result)
                        {
                            if (!paymentsByLic.ContainsKey(debt.Lic))
                                continue;

                            var paymentAmountDouble = Math.Round((double)paymentsByLic[debt.Lic], 2);
                            debt.Payment += paymentAmountDouble;
                            debt.CurrentDebt -= paymentAmountDouble;
                        }
                    }
                }

                return result;
            }
        }
        public async Task<DebtInfoForLic> GetDebtInfoForLicAsync(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

#if DEBUG
                var result = await db.Database.SqlQuery<DebtInfoForLic>($" SELECT * from [Web_App_Test].[dbo].[GetDebtInfoForLic]('{FullLic}')").FirstOrDefaultAsync();
#else
                var result = await db.Database.SqlQuery<DebtInfoForLic>($" SELECT * from [Web_App].[dbo].[GetDebtInfoForLic]('{FullLic}')").FirstOrDefaultAsync();
#endif
                if (result != null)
                    result.Lic = FullLic;

                return result;
            }
        }
        public async Task<List<ManualRecalculationsByFullLic>> GetManualRecalculationsByFullLic (string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

#if DEBUG
                var result = await db.Database.SqlQuery<ManualRecalculationsByFullLic>($" SELECT * from [Billing_Test].[dbo].[GetManualRecalculationsByFullLic]('{FullLic}')").ToListAsync();
#else
                var result = await db.Database.SqlQuery<ManualRecalculationsByFullLic>($" SELECT * from [Billing].[dbo].[GetManualRecalculationsByFullLic]('{FullLic}')").ToListAsync();
#endif
                return result;
            }
        }
        public async Task RemoveRecalculation(Guid Id,int serviceId)
        {
            using (var db = new ApplicationDbContext())
            {
#if DEBUG
                await db.Database.ExecuteSqlCommandAsync($"exec [Billing_Test].[dbo].[DeleteManualRecalculationByGuid] @guid='{Id}', @serviceId={serviceId}");
#else
                await db.Database.ExecuteSqlCommandAsync($"exec [Billing].[dbo].[DeleteManualRecalculationByGuid] @guid='{Id}', @serviceId={serviceId}");
#endif
            }
        }
        public async Task<List<HelpCalculationsModel>> GetInfoHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            var dateFrom = Convert.ToDateTime(DateFrom.ToString("yyyy,MM"));
            var dateTo = Convert.ToDateTime(DateTo.ToString("yyyy,MM")).AddMonths(1);
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var SubLic = FullLic.ConvertLicToSubLic(FullLic);
                    List<HelpCalculationsModel> helpCalculationsModels = new List<HelpCalculationsModel>();
                    var dbLic = new DbLIC();
                    var HelpCalc = db.HelpСalculation.Where(x => x.LIC == FullLic && x.Period >= dateFrom && x.Period <= dateTo).ToListAsync();
                    DateTo = DateTo.AddMonths(1);
                    var Receipt = dbLic.KVIT.Where(x => x.lic == SubLic && x.period.Value >= DateFrom && x.period.Value <= DateTo).Select(x => new
                    {
                        Period = x.period,
                        HeatingRecalculationRate = x.sted2,
                        HeatingСalculationGcal = x.koled2,
                        GvsHeatingRecalculationRate = x.sted3,
                        GvsHeatingСalculationGcal = x.koled3,
                        HvHeatingСalculationGcal = x.sted5,
                        HvHeatingRecalculationRate = x.koled5

                    }).ToListAsync();
                    await Task.WhenAll(HelpCalc, Receipt);
                    helpCalculationsModels = helpCalculationsModels.ConvertToModelHelpCalculations(HelpCalc.Result);
                    foreach (var Item in HelpCalc.Result)
                    {
                        try
                        {
                            var receipt = Receipt.Result.FirstOrDefault(x => x.Period.Value.Month == Item.Period.Value.Month && x.Period.Value.Year == Item.Period.Value.Year);
                            var helpModel = helpCalculationsModels.FirstOrDefault(x => x.Period.Value.Month == Item.Period.Value.Month && x.Period.Value.Year == Item.Period.Value.Year);
                            if (!string.IsNullOrEmpty(receipt?.HeatingRecalculationRate))
                                helpModel.HeatingRecalculationRate = Convert.ToDecimal(receipt?.HeatingRecalculationRate.Replace(".", ","));
                            if (!string.IsNullOrEmpty(receipt?.HeatingСalculationGcal))
                                helpModel.HeatingСalculationGcal = Convert.ToDecimal(receipt?.HeatingСalculationGcal.Replace(".", ","));
                            if (!string.IsNullOrEmpty(receipt?.GvsHeatingRecalculationRate))
                                helpModel.GvsHeatingRecalculationRate = Convert.ToDecimal(receipt?.GvsHeatingRecalculationRate.Replace(".", ","));
                            if (!string.IsNullOrEmpty(receipt?.GvsHeatingСalculationGcal))
                                helpModel.GvsHeatingСalculationGcal = Convert.ToDecimal(receipt?.GvsHeatingСalculationGcal.Replace(".", ","));
                            if (!string.IsNullOrEmpty(receipt?.HvHeatingСalculationGcal))
                                helpModel.HvHeatingСalculationGcal = Convert.ToDecimal(receipt?.HvHeatingСalculationGcal.Replace(".", ","));
                            if (!string.IsNullOrEmpty(receipt?.HvHeatingRecalculationRate))
                                helpModel.HvHeatingRecalculationRate = Convert.ToDecimal(receipt?.HvHeatingRecalculationRate.Replace(".", ","));
                        }
                        catch { }
                    }
                    return helpCalculationsModels.OrderBy(x => x.Period).ToList();
                }
                catch { return new List<HelpCalculationsModel>(); }
            }
        }
        public List<PersData> GetInfoPersData(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Res = db.PersData.Where(x => x.Lic == FullLic && (x.IsDelete == false || x.IsDelete == null)).Include("PersDataDocument").Include(x=>x.Benefit).OrderByDescending(x => x.Main).ToList();
                    return Res;
                }
                catch (Exception ex)
                {
                    var res = ex.InnerException.Message;
                    return null;
                }

            }
        }
        public ALL_LICS GetNoteAllLic(string FullLic)
        {
            using (var db = new DbLIC())
            {
                try
                {
                    var Res = db.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == FullLic);
                    return Res;
                }
                catch (Exception ex)
                {
                    var res = ex.InnerException.Message;
                    return null;
                }

            }
        }
        public List<PersData> GetInfoPersDataDelete(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

                var Res = db.PersData.Where(x => x.Lic == FullLic && x.IsDelete == true).Include("PersDataDocument").Include(x=>x.Benefit).ToList();
                return Res;
            }
        }
        public string GetRoomTypeMain(string Full_Lic)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Res = db.PersData.Where(x => x.Lic == Full_Lic && x.Main == true).Include(x => x.Benefit).First();
                    return Res.RoomType;
                }
                catch
                {
                    return null;
                }
            }
        }
        public string SaveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile, string User)
        {
            Fio.Replace("\"", "");
            if (file != null)
            {
                if (!Directory.Exists($@"\\10.10.10.17\\doc_tplus\\{Lic}\\{Fio}"))
                {
                    Directory.CreateDirectory($@"\\10.10.10.17\\doc_tplus\\{Lic}\\{Fio}");
                }
                if (File.Exists($@"\\10.10.10.17\doc_tplus\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}")) return $@"Файл с название {NameFile} уже существует. Обратитесь к системному администратору!";
                File.WriteAllBytes($@"\\10.10.10.17\\doc_tplus\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}", file);
                using (var db = new ApplicationDbContext())
                {
                    db.PersDataDocument.Add(new PersDataDocument
                    {
                        DocumentPath = $@"{Lic}\\{Fio}",
                        DocumentName = $@"{NameFile}.{TypeFile}",
                        idPersData = idPersData
                    });
                    db.SaveChanges();
                }
                _ilogger.ActionUsersPersData(idPersData, $"Добавил файл {NameFile}.{TypeFile}", User);
            }
            else
            {
                return "Введите название файла";
            }
            return "Файл успешно сохранен";
        }
        public PersDataDocumentLoad DownLoadFile(int Id)
        {
            PersDataDocumentLoad persDataDocument = new PersDataDocumentLoad();
            using (var db = new ApplicationDbContext())
            {
                var Res = db.PersDataDocument.Where(x => x.id == Id).FirstOrDefault();
                persDataDocument.FileBytes = File.ReadAllBytes($@"\\10.10.10.17\\doc_tplus\\{Res.DocumentPath}\\{Res.DocumentName}");
                persDataDocument.FileName = Res.DocumentName;

            }
            return persDataDocument;
        }
        /// <summary>
        /// Справка расчета
        /// </summary>
        /// <param name="FullLic"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public async Task<PersDataDocumentLoad> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            PersDataDocumentLoad persDataDocument = new PersDataDocumentLoad();
            persDataDocument.FileName = $@"Справка расчета {FullLic}.xlsx";
            var dateFrom = Convert.ToDateTime(DateFrom.ToString("yyyy,MM"));
            var dateTo = Convert.ToDateTime(DateTo.ToString("yyyy,MM")).AddMonths(1);
            using (var db = new ApplicationDbContext())
            {
                var Result = await GetInfoHelpСalculation(FullLic, dateFrom, dateTo);
                //var Result = db.HelpСalculation.Where(x => x.LIC == FullLic && x.Period >= dateFrom && x.Period <= dateTo).ToList();
                persDataDocument.FileBytes = ExcelHelpСalculation.Generate(Result);
            }
            return persDataDocument;
        }
        public string DeleteFile(int Id, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var Res = db.PersDataDocument.Where(x => x.id == Id).FirstOrDefault();
                db.PersDataDocument.Remove(Res);
                _ilogger.ActionUsersPersData(Res.idPersData, $"Удалил файл {Res.DocumentName}", User);
                db.SaveChanges();
                File.Delete($@"\\10.10.10.17\\doc_tplus\\{Res.DocumentPath}\\{Res.DocumentName}");
                return $"Файл {Res.DocumentName} успешно удален";
            }
        }
        public List<LogsPersData> GetHistory(int idPersData)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.LogsPersData.Where(x => x.idPersData == idPersData).OrderByDescending(x => x.DateTime).ToList();
            }
        }
        public void UpdatePersDataSquareExcel(PersDataModel persDataModel, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var persInfo = db.PersData.FirstOrDefault(x => x.Lic == persDataModel.Lic && x.Main == true && x.IsDelete == false);
                if (persInfo == null)
                    throw new Exception("Не найден главный перс");
                persDataModel.idPersData = persInfo.idPersData;
                persDataModel.NumberOfPersons = persInfo.NumberOfPersons;
                using (var dbAllLic = new DbLIC())
                {
                    var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == persDataModel.Lic).FirstOrDefault();
                    AllLIC.SOBS = Convert.ToDecimal(persDataModel.Square);
                    dbAllLic.SaveChanges();
                }
                //_ilogger.ActionUsersPersData(persInfo.idPersData, $"Изменили площадь: было {persInfo.Square} стало {persDataModel.Square} \r\n", User);
                var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && (x.IsDelete == false || x.IsDelete == null)).ToList();
                foreach (var Items in ListPers)
                {
                    Items.Square = persDataModel.Square;
                }
                db.SaveChanges();
            }
        }
        public void SavePersonalData(PersDataModel persDataModel, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var PersData = db.PersData.Include(x => x.Benefit).FirstOrDefault(x=>x.idPersData == persDataModel.idPersData);
                _ilogger.ActionUsersPersData(PersData.idPersData, _generatorDescriptons.Generate(persDataModel,true), User);
                if (PersData.Main == true)
                {
                    using (var dbAllLic = new DbLIC())
                    {
                        var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == persDataModel.Lic).FirstOrDefault();
                        AllLIC.KL = persDataModel.NumberOfPersons;
                        AllLIC.SOBS = Convert.ToDecimal(persDataModel.Square);
                        AllLIC.FAMIL = persDataModel.LastName;
                        AllLIC.OTCH = persDataModel.MiddleName;
                        AllLIC.IMYA = persDataModel.FirstName;
                        AllLIC.F4EPLOMBA = persDataModel.FlatTypeId;
                        AllLIC.FIO = $"{persDataModel.LastName} {persDataModel.FirstName?.ToUpper()[0]}.{persDataModel.MiddleName?.ToUpper()[0]}.";
                        dbAllLic.SaveChanges();
                    }
                }
                var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                if (PersData.Main == true)
                {
                    foreach (var Items in ListPers)
                    {
                        Items.Square = persDataModel.Square;
                        Items.NumberOfPersons = persDataModel.NumberOfPersons;
                    }
                }
                db.SaveChanges();
                PersData.SendingElectronicReceipt = string.IsNullOrEmpty(persDataModel.SendingElectronicReceipt) ? PersData.SendingElectronicReceipt : persDataModel.SendingElectronicReceipt;
                PersData.DateAdd = persDataModel.DateAdd == null ? PersData.DateAdd : persDataModel.DateAdd;
                PersData.Comment = persDataModel.Comment;
                PersData.Comment1 = persDataModel.Comment1;
                PersData.Comment2 = persDataModel.Comment2;
                PersData.DateOfBirth = persDataModel.DateOfBirth;
                PersData.Email = persDataModel.Email;
                PersData.FirstName = persDataModel.FirstName;
                PersData.Inn = persDataModel.Inn;
                PersData.IsDelete = persDataModel.IsDelete;
                PersData.LastName = persDataModel.LastName;
                PersData.Lic = persDataModel.Lic;
                PersData.Main = persDataModel.Main;
                PersData.MiddleName = persDataModel.MiddleName;
                PersData.PassportDate = persDataModel.PassportDate;
                PersData.PassportIssued = persDataModel.PassportIssued;
                PersData.PassportNumber = persDataModel.PassportNumber;
                PersData.PassportSerial = persDataModel.PassportSerial;
                PersData.PlaceOfBirth = persDataModel.PlaceOfBirth;
                PersData.RoomType = persDataModel.RoomType;
                PersData.SnilsNumber = persDataModel.SnilsNumber;
                PersData.BenefitId = persDataModel.BenefitId;
                PersData.BenefitEndDate = persDataModel.BenefitEndDate;
                if (PersData.Main == true)
                {
                    PersData.Square = persDataModel.Square;
                    PersData.NumberOfPersons = persDataModel.NumberOfPersons;
                }
                PersData.StateLic = persDataModel.StateLic;
                PersData.Tel1 = persDataModel.Tel1;
                PersData.Tel2 = persDataModel.Tel2;
                PersData.UserName = persDataModel.UserName;
                PersData.DateEdit = DateTime.Now;
                db.SaveChanges();
            }
        }
        public void SavePersonalDataMain(PersDataModel persDataModel, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var PErsData = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main == true && x.IsDelete != true).Include(x => x.Benefit).ToList();
                if (PErsData.Count() == 0)
                {
                    throw new Exception($"На лицевом счете {persDataModel.Lic} нет основного");
                }
                if (PErsData.Count() > 1)
                {
                    throw new Exception($"На лицевом счете {persDataModel.Lic} более одного основного");
                }
                var pers = PErsData.FirstOrDefault();
                var PersData = db.PersData.Find(pers.idPersData);
                persDataModel.idPersData = pers.idPersData;
                if(!string.IsNullOrEmpty(persDataModel.FlatTypeId))
                    persDataModel.FlatType = GetFlatTypeById(persDataModel.FlatTypeId).FlatType;

                _ilogger.ActionUsersPersData(PersData.idPersData, _generatorDescriptons.Generate(persDataModel), User);
                using (var dbAllLic = new DbLIC())
                {
                    var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == persDataModel.Lic).FirstOrDefault();
                    AllLIC.KL = persDataModel.NumberOfPersons != null ? persDataModel.NumberOfPersons : AllLIC.KL;
                    AllLIC.SOBS = persDataModel.Square != null ? Convert.ToDecimal(persDataModel.Square) : AllLIC.SOBS;
                    AllLIC.F4EPLOMBA = string.IsNullOrEmpty(persDataModel.FlatTypeId) ? AllLIC.F4EPLOMBA : persDataModel.FlatTypeId;
                    dbAllLic.SaveChanges();
                }
                var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                foreach (var Items in ListPers)
                {
                    Items.Square = persDataModel.Square != null ? persDataModel.Square : Items.Square;
                    Items.NumberOfPersons = persDataModel.NumberOfPersons != null ? persDataModel.NumberOfPersons : Items.NumberOfPersons;
                }
                db.SaveChanges();
                PersData.SendingElectronicReceipt = string.IsNullOrEmpty(persDataModel.SendingElectronicReceipt) ? PersData.SendingElectronicReceipt : persDataModel.SendingElectronicReceipt;
                PersData.DateAdd = persDataModel.DateAdd == null ? PersData.DateAdd : persDataModel.DateAdd;
                PersData.Comment = string.IsNullOrEmpty(persDataModel.Comment) ? PersData.Comment : persDataModel.Comment;
                PersData.Comment1 = string.IsNullOrEmpty(persDataModel.Comment1) ? PersData.Comment1 : persDataModel.Comment1;
                PersData.Comment2 = string.IsNullOrEmpty(persDataModel.Comment2) ? PersData.Comment2 : persDataModel.Comment2;
                PersData.DateOfBirth = persDataModel.DateOfBirth == null ? PersData.DateOfBirth : persDataModel.DateOfBirth;
                PersData.Email = string.IsNullOrEmpty(persDataModel.Email) ? PersData.Email : persDataModel.Email;
                PersData.FirstName = string.IsNullOrEmpty(persDataModel.FirstName) ? PersData.FirstName : persDataModel.FirstName;
                PersData.Inn = string.IsNullOrEmpty(persDataModel.Inn) ? PersData.Inn : persDataModel.Inn;
                PersData.IsDelete = persDataModel.IsDelete == null ? PersData.IsDelete : persDataModel.IsDelete;
                PersData.LastName = string.IsNullOrEmpty(persDataModel.LastName) ? PersData.LastName : persDataModel.LastName;
                PersData.Lic = string.IsNullOrEmpty(persDataModel.Lic) ? PersData.Lic : persDataModel.Lic;
                PersData.Main = persDataModel.Main == null ? PersData.Main : persDataModel.Main;
                PersData.MiddleName = persDataModel.MiddleName == null ? PersData.MiddleName : persDataModel.MiddleName;
                PersData.NumberOfPersons = persDataModel.NumberOfPersons == null ? PersData.NumberOfPersons : persDataModel.NumberOfPersons;
                PersData.PassportDate = persDataModel.PassportDate == null ? PersData.PassportDate : persDataModel.PassportDate;
                PersData.PassportIssued = string.IsNullOrEmpty(persDataModel.PassportIssued) ? PersData.PassportIssued : persDataModel.PassportIssued;
                PersData.PassportNumber = string.IsNullOrEmpty(persDataModel.PassportNumber) ? PersData.PassportNumber : persDataModel.PassportNumber;
                PersData.PassportSerial = string.IsNullOrEmpty(persDataModel.PassportSerial) ? PersData.PassportSerial : persDataModel.PassportSerial;
                PersData.PlaceOfBirth = string.IsNullOrEmpty(persDataModel.PlaceOfBirth) ? PersData.PlaceOfBirth : persDataModel.PlaceOfBirth;
                PersData.RoomType = string.IsNullOrEmpty(persDataModel.RoomType) ? PersData.RoomType : persDataModel.RoomType;
                PersData.SnilsNumber = string.IsNullOrEmpty(persDataModel.SnilsNumber) ? PersData.SnilsNumber : persDataModel.SnilsNumber;
                PersData.Square = persDataModel.Square == null ? PersData.Square : persDataModel.Square;
                PersData.StateLic = string.IsNullOrEmpty(persDataModel.StateLic) ? PersData.StateLic : persDataModel.StateLic;
                PersData.Tel1 = string.IsNullOrEmpty(persDataModel.Tel1) ? PersData.Tel1 : persDataModel.Tel1;
                PersData.Tel2 = string.IsNullOrEmpty(persDataModel.Tel2) ? PersData.Tel2 : persDataModel.Tel2;
                PersData.UserName = string.IsNullOrEmpty(persDataModel.UserName) ? PersData.UserName : persDataModel.UserName;
                PersData.BenefitId = persDataModel.BenefitId == null ? PersData.BenefitId : persDataModel.BenefitId;
                PersData.BenefitEndDate = persDataModel.BenefitEndDate == null ? PersData.BenefitEndDate : persDataModel.BenefitEndDate;
                db.SaveChanges();
            }
        }
        public void SavePersonalDataFioLic(PersDataModel persDataModel)
        {
            using (var dbAllLic = new DbLIC())
            {
                var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == persDataModel.Lic).FirstOrDefault();
               
                AllLIC.FAMIL = string.IsNullOrEmpty(persDataModel.LastName) ? AllLIC.FAMIL : persDataModel.LastName; ;
                AllLIC.OTCH = string.IsNullOrEmpty(persDataModel.MiddleName) ? AllLIC.OTCH : persDataModel.MiddleName; ; ;
                AllLIC.IMYA = string.IsNullOrEmpty(persDataModel.FirstName) ? AllLIC.IMYA : persDataModel.FirstName; ; ;
                AllLIC.FIO = $"{AllLIC.FAMIL} {AllLIC.IMYA?.ToUpper().TryGetValue(0)}.{AllLIC.OTCH?.ToUpper().TryGetValue(0)}.";
                dbAllLic.SaveChanges();
            }
        }
        public void MakeToMain (int idPersData, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var Main = db.PersData.Find(idPersData);
                _ilogger.ActionUsersPersData(idPersData, "Сделал основным", User);
                var AllPers = db.PersData.Where(x => x.Lic == Main.Lic && x.IsDelete != true).ToList();
                foreach(var Items in AllPers)
                {
                    Items.Main = false;
                    if (Items.idPersData == idPersData)
                    {
                        Items.Main = true;
                        using(var dbAllLic = new DbLIC())
                        {
                            var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == Items.Lic).FirstOrDefault();
                            AllLIC.KL = Items.NumberOfPersons != null ? Convert.ToDecimal(Items.NumberOfPersons) : 0;
                            AllLIC.SOBS = Items.Square != null ? Convert.ToDecimal(Items.Square) : 0;
                            AllLIC.FAMIL = Items.LastName;
                            AllLIC.OTCH = Items.MiddleName;
                            AllLIC.IMYA = Items.FirstName;
                            AllLIC.FIO = $"{Items.LastName} {Items.FirstName?.ToUpper()[0]}.{Items.MiddleName?.ToUpper()[0]}.";
                            dbAllLic.SaveChanges();
                        }
                    }
                }
                db.SaveChanges();
                var PersMain = db.PersData.Where(x => x.Lic == Main.Lic && x.Main == true && (x.IsDelete == false || x.IsDelete == null))?.FirstOrDefault();
                if (PersMain != null)
                {
                    var ListPers = db.PersData.Where(x => x.Lic == Main.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                    foreach (var Items in ListPers)
                    {
                        Items.Square = PersMain.Square;
                        Items.NumberOfPersons = PersMain.NumberOfPersons;
                    }
                    db.SaveChanges();
                }
            }
        }
        public void AddPersData(PersDataModel persDataModel, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                PersData persData = ConvertToModel.PersDataModel_To_PersData(persDataModel);
                db.PersData.Add(persData);
                db.SaveChanges();
                _ilogger.ActionUsersPersData(persData.idPersData, "Добавил", User);
                var PersMain = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main == true && (x.IsDelete == false || x.IsDelete == null))?.FirstOrDefault();
                if (PersMain != null)
                {
                    var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                    foreach (var Items in ListPers)
                    {
                        Items.Square = PersMain.Square;
                        Items.NumberOfPersons = PersMain.NumberOfPersons;
                    }
                    db.SaveChanges();
                }
            }
        }
        public void DeletePers(int IdPersData, string User)
        {
            _ilogger.ActionUsersPersData(IdPersData, "Удалил", User);
            using (var db = new ApplicationDbContext())
            {
                var Pers = db.PersData.Find(IdPersData);
                Pers.IsDelete = true;
                Pers.SendingElectronicReceipt = "Нет";
                db.SaveChanges();
            }
        }
        public List<PaymentHistoryResponse> GetPaymentHistory(string Full_Lic)
        {
            using (var db = new DbPaymentV2())
            {
                using (var dbArchive = new DbPaymentV2Archive())
                {
                    try
                    {
                        var payments = db.Payments.Include(x => x.Counters).Include(x => x.Banks).Where(x => x.Lic == Full_Lic).ToList();
                        var paymentsArchive = dbArchive.Payments.Where(x=>x.Lic == Full_Lic).ToList();

                        var payment = _mapper.Map<List<PaymentHistoryResponse>>(payments);
                        payment.AddRange(_mapper.Map<List<PaymentHistoryResponse>>(paymentsArchive));
                        return payment;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            
        }
        public List<ReadingsHistoryResponse> GetReadingsHistory(string Full_Lic)
        {
            using (var db = new DbPaymentV2())
            {
                using (var dbArchive = new DbPaymentV2Archive())
                {
                    try
                    {
                        var readingsHistory = new List<ReadingsHistoryResponse>();
                        var payments = db.Payments.Include(x => x.Counters).Include(x => x.Banks).Where(x => x.Lic == Full_Lic).ToList();
                        var paymentsArchive = dbArchive.Payments.Include(x=>x.Counters).Where(x => x.Lic == Full_Lic).ToList();
                        foreach (var item in payments)
                            foreach (var counter in item.Counters)
                                readingsHistory.Add(new ReadingsHistoryResponse
                                {
                                    Name = counter.Name,
                                    OrganizationName = item.Banks.Name,
                                    PaymentDateDay = item.PaymentDateDay,
                                    Value = counter.Value,
                                });
                        foreach (var item in paymentsArchive)
                            foreach (var counter in item.Counters)
                                readingsHistory.Add(new ReadingsHistoryResponse
                                {
                                    Name = counter.Name,
                                    OrganizationName = item.RegisterBankName,
                                    PaymentDateDay = item.PaymentDateDay,
                                    Value = counter.Value,
                                });
                        return readingsHistory;
                        
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
           
        }
        public List<DB.Model.Counters> GetReadingsHistorySearch(string Parametr, string Full_Lic)
        {
            using (var db = new DbPayment())
            {
                IQueryable<DB.Model.Counters> res = db.Counter.Include(x => x.Payment).Where(x => x.lic == Full_Lic && x.name == Parametr);
                return res.ToList();
            }
        }
        public void UpdateSquareFlat(double? Square, string Lic)
        {
            using(var db = new DbTPlus())
            {
                //var flat = db.FLAT.Where(x => x.object_id == Lic).ToList();
                //foreach(var Items in flat)
                //{
                //    Items.square_all = Square;
                //    Items.date_edit = DateTime.Now.Date;
                //}
                //db.SaveChanges();
            }
        }
        public void UpdateSquareCadastrFlat(double? Square,string Cadastr, string Lic)
        {
            using (var db = new DbTPlus())
            {
                //var flat = db.FLAT.Where(x => x.object_id == Lic).ToList();
                //foreach (var Items in flat)
                //{
                //    Items.square_all = Square;
                //    Items.date_edit = DateTime.Now.Date;
                //    Items.cadastral_number = !string.IsNullOrEmpty(Cadastr) ? Cadastr : Items.cadastral_number;
                //}
                //db.SaveChanges();
            }
        }
        private async Task ResetNumberOfPersonsAndSquarePers(string FullLic, string User = "")
        {
            using (var appDb = new ApplicationDbContext())
            {
                var AllPers = await appDb.PersData.Where(x => x.Lic == FullLic).ToListAsync();
                foreach(var Item in AllPers)
                {
                    Item.Square = 0;
                    Item.NumberOfPersons = 0;
                    Item.SendingElectronicReceipt = null;
                    if(!string.IsNullOrEmpty(User))
                        _ilogger.ActionUsersPersData(Item.idPersData, $"Закрыл", User);
                }
                await appDb.SaveChangesAsync();
            }
        }
        public void CloseLic(string FullLic, ICounter _counter, string User)
        {
            using (var dbLic = new DbLIC())
            {
                var lic = dbLic.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == FullLic);
                lic.ZAK = "0";
                dbLic.SaveChanges();
                var Ipu = _counter.DetailInfroms(FullLic);
                foreach(var Item in Ipu)
                {
                    _counter.DeleteIPU(Item.ID_PU);
                    _ilogger.ActionUsers(Item.ID_PU, "Закрыл", User);
                }
                Task.Run(() => ResetNumberOfPersonsAndSquarePers(FullLic,User));
                Task.Run(() => ClosePersInLic(FullLic));
            }
        }
        public async Task CloseLicAsync(string FullLic,string Description, ICounter _counter, string User)
        {
            using (var dbLic = new DbLIC())
            {
                var lic = dbLic.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == FullLic);
                lic.ZAK = "0";
                dbLic.SaveChanges();
                var Ipu = _counter.DetailInfroms(FullLic);
                foreach (var Item in Ipu)
                {
                    _counter.DeleteIPU(Item.ID_PU);
                    _ilogger.ActionUsers(Item.ID_PU, "Закрыл", User);
                }
                await ResetNumberOfPersonsAndSquarePers(FullLic,User);
                await ClosePersInLic(FullLic);
            }
        }
        public void OpenLic(string FullLic)
        {
            using (var dbLic = new DbLIC())
            {
                var lic = dbLic.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == FullLic);
                lic.ZAK = null;
                dbLic.SaveChanges();
            }
        }

        public async Task<List<ALL_LICS_ARCHIVE>> GetHistoryAccrualsByItems(string fullLic)
        {
            using (var dbLic = new DbLIC())
            {
                var lics = await dbLic.ALL_LICS_ARCHIVE.Where(x => x.F4ENUMELS == fullLic)
                    //.Select(x=> new ALL_LICS_ARCHIVE
                    //{
                    //    period = x.period,
                    //    SN2 = x.SN2,
                    //    SR2 = x.SR2,
                    //    SN3 = x.SN3,
                    //    SR3 = x.SR3,
                    //    SN4 = x.SN4,
                    //    SR4 = x.SR4,
                    //    SN5 = x.SN5,
                    //    SR5 = x.SR5,
                    //    SN6 = x.SN6,
                    //    SR6 = x.SR6,
                    //    SN7 = x.SN7,
                    //    SR7 = x.SR7,
                    //    SN8 = x.SN8,
                    //    SR8 = x.SR8,
                    //    SN9 = x.SN9,
                    //    SR9 = x.SR9,
                    //    SN10 = x.SN10,
                    //    SR10 = x.SR10,
                    //    SN11 = x.SN11,
                    //    SR11 = x.SR11,
                    //    SN12 = x.SN12,
                    //    SR12 = x.SR12,
                    //    SN13 = x.SN13,
                    //    SR13 = x.SR13,
                    //    SN14 = x.SN14,
                    //    SR14 = x.SR14,
                    //    SN15 = x.SN15,
                    //    SR15 = x.SR15,
                    //    PENY_SN = x.PENY_SN,
                    //    PENY_SR = x.PENY_SR,
                    //})
                    .ToListAsync();
                return lics;
            }
        }
    }
}
