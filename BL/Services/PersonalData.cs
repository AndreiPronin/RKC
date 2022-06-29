using BE.PersData;
using BL.Helper;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
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
    public interface IPersonalData
    {
        List<PersonalInformation> GetPersonalInformation(string FullLic);
        List<PersData> GetInfoPersData(string FullLic);
        List<PersData> GetInfoPersDataDelete(string FullLic);
        string saveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile, string User);
        PersDataDocumentLoad DownLoadFile(int Id);
        PersDataDocumentLoad DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
        string DeleteFile(int Id, string User);
        List<LogsPersData> GetHistory(int idPersData);
        void SavePersonalData(PersDataModel persDataModelView, string User);
        void MakeToMain(int idPersData);
        void AddPersData(PersDataModel persDataModel, string User);
        void DeletePers(int IdPersData, string User);
        string GetRoomTypeMain(string Full_Lic);

    }
    public class PersonalData : IPersonalData
    {
        Ilogger _ilogger;
        IGeneratorDescriptons _generatorDescriptons;
        public PersonalData(Ilogger ilogger, IGeneratorDescriptons generatorDescriptons)
        {
            _ilogger = ilogger;
            _generatorDescriptons = generatorDescriptons;
        }
        public List<PersonalInformation> GetPersonalInformation(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                var tttt = db.PersonalInformation.Where(x => x.full_lic == FullLic).ToList();
                return db.PersonalInformation.Where(x => x.full_lic == FullLic).ToList();
            }
        }
        public List<PersData> GetInfoPersData(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Res = db.PersData.Where(x => x.Lic == FullLic && (x.IsDelete == false || x.IsDelete == null)).Include("PersDataDocument").OrderByDescending(x => x.Main).ToList();
                    return Res;
                }
                catch(Exception ex)
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

                var Res = db.PersData.Where(x => x.Lic == FullLic && x.IsDelete == true).Include("PersDataDocument").ToList();
                return Res;
            }
        }
        public string GetRoomTypeMain(string Full_Lic)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Res = db.PersData.Where(x => x.Lic == Full_Lic && x.Main == true).First();
                    return Res.RoomType;
                }
                catch
                {
                    return null;
                }
            }
        }
        public string saveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile, string User)
        {
            /* string[] dirs = Directory.GetDirectories("10.10.10.6\\doc_tplus\\")*/
            ;
            if (file != null)
            {
                if (!Directory.Exists($@"\\10.10.10.6\\doc_tplus\\{Lic}\\{Fio}"))
                {
                    Directory.CreateDirectory($@"\\10.10.10.6\\doc_tplus\\{Lic}\\{Fio}");
                }
                if (File.Exists($@"\\10.10.10.6\doc_tplus\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}")) return $@"Файл с название {NameFile} уже существует. Обратитесь к системному администратору!";
                File.WriteAllBytes($@"\\10.10.10.6\\doc_tplus\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}", file);
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
                persDataDocument.FileBytes = File.ReadAllBytes($@"\\10.10.10.6\\doc_tplus\\{Res.DocumentPath}\\{Res.DocumentName}");
                persDataDocument.FileName = Res.DocumentName;

            }
            return persDataDocument;
        }
        public PersDataDocumentLoad DownLoadHelpСalculation(string FullLic,DateTime DateFrom, DateTime DateTo)
        {
            PersDataDocumentLoad persDataDocument = new PersDataDocumentLoad();
            persDataDocument.FileName = $@"Справка расчета {FullLic}.xlsx";
            var dateFrom = Convert.ToDateTime(DateFrom.ToString("yyyy,MM"));
            var dateTo = Convert.ToDateTime(DateTo.ToString("yyyy,MM")).AddMonths(1);
            using (var db = new ApplicationDbContext())
            {
                var Result = db.HelpСalculation.Where(x => x.FULL_LIC == FullLic && x.Period >= dateFrom && x.Period <= dateTo).ToList();
                using (var workbook = new XLWorkbook())
                {
                    int ComnNumber = 1;
                    var worksheet = workbook.Worksheets.Add("Лист1");
                    worksheet.Cell(1, ComnNumber++).Value = "Период";
                    worksheet.Cell(1, ComnNumber++).Value = "Сальдо на начало периода -ЖКУ";
                    worksheet.Cell(1, ComnNumber++).Value = "Сальдо на начало периода -ПЕНИ";
                    worksheet.Cell(1, ComnNumber++).Value = "Начислено, в т.ч.перерасчет -ЖКУ";
                    worksheet.Cell(1, ComnNumber++).Value = "Начислено, в т.ч.перерасчет -ПЕНИ";
                    worksheet.Cell(1, ComnNumber++).Value = "Оплачено -ЖКУ";
                    worksheet.Cell(1, ComnNumber++).Value = "Оплачено -ПЕНИ";
                    worksheet.Cell(1, ComnNumber++).Value = "К оплате -ЖКУ";
                    worksheet.Cell(1, ComnNumber++).Value = "К оплате -ПЕНИ";
                    worksheet.Cell(1, ComnNumber++).Value = "К оплате";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление -Начислено";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление -Перерасчет";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС компонент ТЭ -Начислено";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС компонент ТЭ -Перерасчет";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС компонент ХВ -Начислено";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС компонент ХВ -Перерасчет";
                    worksheet.Cell(1, ComnNumber++).Value = "Корректир.с.альдо -ЖКУ";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС1 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС1 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС2 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС2 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС3 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС3 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС4 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "ГВС4 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление1 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление1 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление2 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление2 -Кон.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление3 -Нач.пок";
                    worksheet.Cell(1, ComnNumber++).Value = "Отопление3 -Кон.пок";
                    worksheet.RangeUsed().SetAutoFilter();
                    worksheet.Columns().AdjustToContents();
                    int RowNumber = 1;
                    foreach (var Items in Result)
                    {
                        ComnNumber = 1;
                        RowNumber++;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.Period;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.DK;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.PENY_DK;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.SumSn;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.SumPenySnSr;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.SP;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.PENY;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.TDK;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.PenyTdk;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.SumTdkPeny_tdk;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.HeatingСalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.HeatingRecalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.GvsHeatingСalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.GvsHeatingRecalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.HvHeatingСalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.HvHeatingRecalculation;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.SN15;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1XVS;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2XVS;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1XV_2;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2XV_2;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1XV_3;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2XV_3;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1XV_4;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2XV_4;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1OT_1;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2OT_1;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1OT_2;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2OT_2;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB1OT_3;
                        worksheet.Cell(RowNumber, ComnNumber++).Value = Items.FKUB2OT_3;
                    }
                    MemoryStream m = new MemoryStream();
                    workbook.SaveAs(m);
                    persDataDocument.FileBytes = m.ToArray();
                }

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
                File.Delete($@"\\10.10.10.6\\doc_tplus\\{Res.DocumentPath}\\{Res.DocumentName}");
                return $"Файл {Res.DocumentName} успешно удален";
            }
        }
        public List<LogsPersData> GetHistory(int idPersData)
        {
            using (var db = new ApplicationDbContext())
            {
               return   db.LogsPersData.Where(x => x.idPersData == idPersData).OrderByDescending(x=>x.DateTime).ToList();
            }
        }
        public void SavePersonalData(PersDataModel persDataModel, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var PersData = db.PersData.Find(persDataModel.idPersData);
                if (СomparisonModel.PersDataModel_To_PersData(PersData, persDataModel))
                {
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
                            AllLIC.FIO = $"{persDataModel.LastName} {persDataModel.FirstName.ToUpper()[0]}.{persDataModel.MiddleName.ToUpper()[0]}.";
                            
                            dbAllLic.SaveChanges();
                        }
                    }
                }
                _ilogger.ActionUsersPersData(PersData.idPersData, _generatorDescriptons.Generate(persDataModel), User);
                //if(persDataModel.Main == true && (persDataModel.Square != PersData.Square 
                //    || persDataModel.NumberOfPersons != PersData.NumberOfPersons))
                //{
                    var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                    foreach(var Items in ListPers)
                    {
                        Items.Square = persDataModel.Square;
                        Items.NumberOfPersons = persDataModel.NumberOfPersons;
                    }
                    db.SaveChanges();
                //}
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
                PersData.NumberOfPersons = persDataModel.NumberOfPersons;
                PersData.PassportDate = persDataModel.PassportDate;
                PersData.PassportIssued = persDataModel.PassportIssued;
                PersData.PassportNumber = persDataModel.PassportNumber;
                PersData.PassportSerial = persDataModel.PassportSerial;
                PersData.PlaceOfBirth = persDataModel.PlaceOfBirth;
                PersData.RoomType = persDataModel.RoomType;
                PersData.SnilsNumber = persDataModel.SnilsNumber;
                PersData.Square = persDataModel.Square;
                PersData.StateLic = persDataModel.StateLic;
                PersData.Tel1 = persDataModel.Tel1;
                PersData.Tel2 = persDataModel.Tel2;
                PersData.UserName = persDataModel.UserName;
                
                db.SaveChanges();
            }
        }
        public void MakeToMain (int idPersData)
        {
            using (var db = new ApplicationDbContext())
            {
                var Main = db.PersData.Find(idPersData);
                var AllPers = db.PersData.Where(x => x.Lic == Main.Lic).ToList();
                foreach(var Items in AllPers)
                {
                    Items.Main = false;
                    if (Items.idPersData == idPersData)
                    {
                        Items.Main = true;
                        using(var dbAllLic = new DbLIC())
                        {
                            try
                            {
                                var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == Items.Lic).FirstOrDefault();
                                AllLIC.KL = Items.NumberOfPersons;
                                AllLIC.SOBS = Convert.ToDecimal(Items.Square);
                                AllLIC.FAMIL = Items.LastName;
                                AllLIC.OTCH = Items.MiddleName;
                                AllLIC.IMYA = Items.FirstName;
                                AllLIC.FIO = $"{Items.LastName} {Items.FirstName} {Items.MiddleName}";
                                dbAllLic.SaveChanges();
                            }
                            catch { }
                        }
                    }
                }
                db.SaveChanges();

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
                db.SaveChanges();
            }
        }
    }
}
