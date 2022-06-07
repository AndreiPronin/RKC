using BE.PersData;
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
        string saveFile(byte[] file, int idPersData, string Fio, string Lic, string TypeFile, string NameFile);
        PersDataDocumentLoad DownLoadFile(int Id);
        string DeleteFile(int Id);
    }
    public class PersonalData : IPersonalData
    {
        public List<PersData> GetInfoPersData(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {

                    var Res = db.PersData.Where(x => x.Lic == FullLic && x.IsDelete == false).Include("PersDataDocument").ToList();
                    return Res;
            }
        }
        public string saveFile(byte[] file, int idPersData, string Fio,string Lic,string TypeFile, string NameFile)
        {
           /* string[] dirs = Directory.GetDirectories("Z:\\")*/;
            if (file != null)
            {
                if (!Directory.Exists($@"Z:\\{Lic}\\{Fio}"))
                {
                    Directory.CreateDirectory($@"Z:\\{Lic}\\{Fio}");
                }
                if (File.Exists($@"Z:\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}")) return $@"Файл с название {NameFile} уже существует. Обратитесь к системному администратору!";
                File.WriteAllBytes($@"Z:\\{Lic}\\{Fio}\\{NameFile}.{TypeFile}",file);
                using (var db = new ApplicationDbContext())
                {
                    db.PersDataDocument.Add(new PersDataDocument
                    {
                         DocumentPath = $@"{Lic}\\{Fio}",
                          DocumentName =$@"{NameFile}.{TypeFile}",
                           idPersData = idPersData
                    });
                    db.SaveChanges();
                }
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
                persDataDocument.FileBytes = File.ReadAllBytes($@"Z:\\{Res.DocumentPath}\\{Res.DocumentName}");
                persDataDocument.FileName = Res.DocumentName;
            }
            return persDataDocument;
        }
        public string DeleteFile(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var Res = db.PersDataDocument.Where(x => x.id == Id).FirstOrDefault();
                db.PersDataDocument.Remove(Res);
                db.SaveChanges();
                File.Delete($@"Z:\\{Res.DocumentPath}\\{Res.DocumentName}");
                return $"Файл {Res.DocumentName} успешно удален";
            }
        }
    }
}
