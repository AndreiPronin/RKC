using BE.PersData;
using BL.Helper;
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
        List<PersData> GetInfoPersData(string FullLic);
        List<PersData> GetInfoPersDataDelete(string FullLic);
        string saveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile, string User);
        PersDataDocumentLoad DownLoadFile(int Id);
        string DeleteFile(int Id, string User);
        List<LogsPersData> GetHistory(int idPersData);
        void SavePersonalData(PersDataModel persDataModelView, string User);
        void MakeToMain(int idPersData);
        void AddPersData(PersDataModel persDataModel, string User);
        void DeletePers(int IdPersData, string User);

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
        public List<PersData> GetInfoPersData(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

                var Res = db.PersData.Where(x => x.Lic == FullLic && (x.IsDelete == false || x.IsDelete == null)).Include("PersDataDocument").OrderByDescending(x => x.Main).ToList();
                return Res;
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
                            AllLIC.FIO = $"{persDataModel.MiddleName} {persDataModel.FirstName} {persDataModel.LastName}";
                            dbAllLic.SaveChanges();
                        }
                    }
                }
                _ilogger.ActionUsersPersData(PersData.idPersData, _generatorDescriptons.Generate(persDataModel), User);
                if(persDataModel.Main == true && (persDataModel.Square != PersData.Square 
                    || persDataModel.NumberOfPersons != PersData.NumberOfPersons))
                {
                    var ListPers = db.PersData.Where(x => x.Lic == persDataModel.Lic && x.Main != true && (x.IsDelete == false || x.IsDelete == null)).ToList();
                    foreach(var Items in ListPers)
                    {
                        Items.Square = persDataModel.Square;
                        Items.NumberOfPersons = persDataModel.NumberOfPersons;
                    }
                    db.SaveChanges();
                }
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
                            var AllLIC = dbAllLic.ALL_LICS.Where(x => x.F4ENUMELS == Items.Lic).FirstOrDefault();
                            AllLIC.KL = Items.NumberOfPersons;
                            AllLIC.SOBS = Convert.ToDecimal(Items.Square);
                            AllLIC.FAMIL = Items.MiddleName;
                            AllLIC.OTCH = Items.LastName;
                            AllLIC.IMYA = Items.FirstName;
                            AllLIC.FIO = $"{Items.FirstName} {Items.LastName} {Items.MiddleName}";
                            dbAllLic.SaveChanges();
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
                db.PersData.Add(ConvertToModel.PersDataModel_To_PersData(persDataModel));
                db.SaveChanges();
            }
        }

        public void DeletePers(int IdPersData, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var Pers = db.PersData.Find(IdPersData);
                Pers.IsDelete = true;
                db.SaveChanges();
            }
        }
    }
}
