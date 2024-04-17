using BE.Counter;
using BE.PersData;
using BL.Counters;
using BL.Excel;
using BL.Extention;
using BL.Helper;
using DB.DataBase;
using DB.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        List<Payment> GetPaymentHistory(string Full_Lic);
        List<Payment> GetReadingsHistory(string Full_Lic);
        void CloseLic(string FullLic, ICounter _counter, string User);
        void OpenLic(string FullLic);
        List<DB.Model.Counters> GetReadingsHistorySearch(string Parametr,string Full_Lic);
        void UpdateSquareFlat(double? Square, string Lic);
        void UpdatePersDataSquareExcel(PersDataModel persDataModel, string User);
        void SavePersonalDataFioLic(PersDataModel persDataModel);
        Task CloseLicAsync(string FullLic, string Description, ICounter _counter, string User);
    }
    public class PersonalData : BaseService, IPersonalData
    {
        private readonly Ilogger _ilogger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly IDictionary _dictionary;
        public PersonalData(Ilogger ilogger, IGeneratorDescriptons generatorDescriptons, IDictionary dictionary)
        {
            _ilogger = ilogger;
            _generatorDescriptons = generatorDescriptons;
            _dictionary = dictionary;
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
                var PersData = db.PersData.Find(persDataModel.idPersData);
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
                PersData.SendingElectronicReceipt = persDataModel.SendingElectronicReceipt;
                PersData.DateAdd = persDataModel.DateAdd;
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
                var PErsData = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main == true && x.IsDelete != true).ToList();
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
        public List<Payment> GetPaymentHistory(string Full_Lic)
        {
            using (var db = new DbPayment())
            {
                return db.Payment.Include(x => x.Counter).Include(x => x.Organization).Where(x => x.lic == Full_Lic).ToList();
            }
        }
        public List<Payment> GetReadingsHistory(string Full_Lic)
        {
            using (var db = new DbPayment())
            {
                return db.Payment.Include(x => x.Counter).Include(x => x.Organization).Where(x => x.lic == Full_Lic).ToList();
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
    }
}
