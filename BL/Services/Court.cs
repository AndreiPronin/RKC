using AutoMapper;
using BE.Court;
using BE.PersData;
using BL.Helper;
using DB.DataBase;
using DB.Model;
using DB.Model.Court;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;
using CourtGeneralInformation = DB.Model.Court.CourtGeneralInformation;

namespace BL.Services
{
    public interface ICourt
    {
        Task<CourtGeneralInformation> DetailInfroms(int Id);
        Task<List<CourtGeneralInformation>> Serach(SearchModel searchModel);
        Task<int> CreateCourt(string FullLic, string NumberIP);
        Task<int> SaveCourt(BE.Court.CourtGeneralInformation courtGeneralInformation, string User);
        Task<List<CourtGeneralInformation>> GetAllCourtFullLic(string FullLic);
        Task AddCourtWorkRequisites (BE.Court.CourtWorkRequisites courtWorkRequisites);
        Task RemoveCourtWorkRequisites(int id);
        Task<List<DB.Model.Court.CourtWorkRequisites>> GetCourtWorkRequisites(int CourtGeneralInformationId);
        Task DeleteCourt(int Id);
        Task<string> saveFile(byte[] file, int CourtGeneralId, string Lic, string TypeFile, string NameFile, string User);
        Task<string> DeleteFile(int Id, string User);
        Task<List<DB.Model.Court.CourtDocumentScans>> GetDocumentScans(int CourtGeneralInformationId);
        Task<CourtDataDocumentLoad> DownLoadFile(int Id);
    }
    public class Court : ICourt
    {
        private readonly IPersonalData _personalData;
        private readonly Ilogger _ilogger;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        public Court(IPersonalData personalData, Ilogger ilogger, IGeneratorDescriptons generatorDescriptons)
        {
            _personalData = personalData;
            _ilogger = ilogger;
            _generatorDescriptons = generatorDescriptons;
        }
        public async Task<CourtGeneralInformation> DetailInfroms(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtGeneralInformation.Where(x=>x.Id == Id).Include(x => x.CourtBankruptcy)
                    .Include(x => x.CourtInstallmentPlan)
                    .Include(x => x.CourtExecutionInPF)
                    .Include(x => x.CourtLitigationWork)
                    .Include(x => x.CourtWork)
                    .Include(x => x.CourtWriteOff)
                    .Include(x => x.CourtStateDuty)
                    .Include(x => x.CourtExecutionFSSP)
                    .Include(x=>x.CourtWorkRequisites)
                    .Include(c => c.CourtDocumentScans).FirstOrDefaultAsync();
                return Result;
            }
        }
        public async Task<int> SaveCourt(BE.Court.CourtGeneralInformation courtGeneralInformation, string User)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                dbApp.DisabledProxy();
                var courtGeneralInformationDb = await dbApp.CourtGeneralInformation.Where(x=>x.Id == courtGeneralInformation.Id).Include(x => x.CourtBankruptcy)
                    .Include(x => x.CourtInstallmentPlan)
                    .Include(x => x.CourtExecutionInPF)
                    .Include(x => x.CourtLitigationWork)
                    .Include(x => x.CourtWork)
                    .Include(x => x.CourtWriteOff)
                    .Include(x => x.CourtStateDuty)
                    .Include(x => x.CourtExecutionFSSP).AsNoTracking().FirstOrDefaultAsync();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<BE.Court.CourtGeneralInformation, DB.Model.Court.CourtGeneralInformation>();
                    cfg.CreateMap<BE.Court.CourtWork, DB.Model.Court.CourtWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                    cfg.CreateMap<BE.Court.CourtBankruptcy, DB.Model.Court.CourtBankruptcy>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                    cfg.CreateMap<BE.Court.CourtInstallmentPlan, DB.Model.Court.CourtInstallmentPlan>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                    cfg.CreateMap<BE.Court.CourtLitigationWork, DB.Model.Court.CourtLitigationWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                    cfg.CreateMap<BE.Court.CourtWriteOff, DB.Model.Court.CourtWriteOff>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                    cfg.CreateMap<BE.Court.CourtStateDuty, DB.Model.Court.CourtStateDuty>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                    cfg.CreateMap<BE.Court.CourtExecutionInPF, DB.Model.Court.CourtExecutionInPF>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                    cfg.CreateMap<BE.Court.CourtExecutionFSSP, DB.Model.Court.CourtExecutionFSSP>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                });
                _ilogger.ActionUserCourt(courtGeneralInformation.Lic, courtGeneralInformation.Id,
                    _generatorDescriptons.Generate(courtGeneralInformation,courtGeneralInformationDb, User));
                var mapper = new Mapper(config);
                courtGeneralInformationDb = mapper.Map<BE.Court.CourtGeneralInformation, DB.Model.Court.CourtGeneralInformation>(courtGeneralInformation);
                courtGeneralInformationDb.CourtBankruptcy.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtInstallmentPlan.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtExecutionInPF.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtLitigationWork.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtWork.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtWriteOff.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtStateDuty.CourtGeneralInformationId = courtGeneralInformation.Id;
                courtGeneralInformationDb.CourtExecutionFSSP.CourtGeneralInformationId = courtGeneralInformation.Id;
                dbApp.Entry(courtGeneralInformationDb).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtBankruptcy).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtInstallmentPlan).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtExecutionInPF).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtLitigationWork).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtWork).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtWriteOff).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtStateDuty).State = EntityState.Modified;
                dbApp.Entry(courtGeneralInformationDb.CourtExecutionFSSP).State = EntityState.Modified;
                dbApp.SaveChanges();
                return courtGeneralInformation.Id;
            }
        }
        public async Task<int> CreateCourt(string FullLic,string DateCreate)
        {
            using (var db = new ApplicationDbContext())
            {
                var PersGeneral = _personalData.GetPersonalInformation(FullLic).FirstOrDefault();
                var PersMain = _personalData.GetInfoPersData(FullLic).Where(x=>x.Main == true).FirstOrDefault();
                var Model = new CourtGeneralInformation { DateCreate = DateCreate };
                Model.Lic = FullLic;
                if (PersGeneral != null)
                {
                    Model.Street = PersGeneral.Street;
                    Model.Home = PersGeneral.House;
                    Model.Flat = PersGeneral.Flat;
                    Model.Home = PersGeneral.House;
                    Model.FioDuty = $"{PersMain.LastName} {PersMain.FirstName} {PersMain.MiddleName}";
                    Model.DateBirthday = PersMain.DateOfBirth.HasValue ? PersMain.DateOfBirth.Value.ToString() : "";
                    Model.PasportDate = PersMain.PassportDate.HasValue ? PersMain.PassportDate.Value.ToString() : "";
                    Model.PasportSeria = PersMain.PassportSerial;
                    Model.PasportNumber = PersMain.PassportNumber;
                    Model.PasportIssue = PersMain.PassportIssued;
                    Model.Inn = PersMain.Inn;
                    Model.Snils = PersMain.SnilsNumber;
                }
                db.CourtGeneralInformation.Add(Model);
                await db.SaveChangesAsync();
                var Id = Model.Id;
                db.CourtBankruptcy.Add(new DB.Model.Court.CourtBankruptcy { CourtGeneralInformationId = Id });
                db.CourtInstallmentPlan.Add(new DB.Model.Court.CourtInstallmentPlan { CourtGeneralInformationId = Id });
                db.CourtExecutionInPF.Add(new DB.Model.Court.CourtExecutionInPF { CourtGeneralInformationId = Id });
                db.CourtLitigationWork.Add(new DB.Model.Court.CourtLitigationWork { CourtGeneralInformationId = Id });
                db.CourtWork.Add(new DB.Model.Court.CourtWork { CourtGeneralInformationId = Id });
                db.CourtWriteOff.Add(new DB.Model.Court.CourtWriteOff { CourtGeneralInformationId = Id });
                db.CourtStateDuty.Add(new DB.Model.Court.CourtStateDuty { CourtGeneralInformationId = Id });
                db.CourtExecutionFSSP.Add(new DB.Model.Court.CourtExecutionFSSP { CourtGeneralInformationId = Id });
                await db.SaveChangesAsync();
                return Model.Id;
            }
        }
        public async Task<List<CourtGeneralInformation>> GetAllCourtFullLic(string FullLic)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<CourtGeneralInformation> query = db.CourtGeneralInformation.Where(x => x.Lic == FullLic).Include(x => x.CourtWork)
                    .Include(x => x.CourtLitigationWork).Include(x => x.CourtExecutionFSSP);
                return await query.ToListAsync();
            }
        }
        public async Task<List<CourtGeneralInformation>> Serach(SearchModel searchModel)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    IQueryable<CourtGeneralInformation> query = db.CourtGeneralInformation.Include(x=>x.CourtWork).Include(x=>x.CourtLitigationWork).Include(x=>x.CourtExecutionFSSP);
                    if (!string.IsNullOrEmpty(searchModel.Lic))
                        query = query.Where(x => x.Lic.Contains(searchModel.Lic));
                    if (!string.IsNullOrEmpty(searchModel.Street))
                        query = query.Where(x => x.Street.Contains(searchModel.Street));
                    if (!string.IsNullOrEmpty(searchModel.Home))
                        query = query.Where(x => x.Home.Contains(searchModel.Home));
                    if (!string.IsNullOrEmpty(searchModel.Flat))
                        query = query.Where(x => x.Flat.Contains(searchModel.Flat));
                    if (!string.IsNullOrEmpty(searchModel.NumberSp))
                        query = query.Where(x => x.CourtWork.NumberSP.Contains(searchModel.NumberSp));
                    if (!string.IsNullOrEmpty(searchModel.NumberIl))
                        query = query.Where(x => x.CourtLitigationWork.NumberIl.Contains(searchModel.NumberIl));
                    if (!string.IsNullOrEmpty(searchModel.NumberIp))
                        query = query.Where(x => x.CourtExecutionFSSP.NumberIP.Contains(searchModel.NumberIp));

                    return await query.Take(30).ToListAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
                //var Result = await db.CourtGeneralInformation.Where(x => x.Lic == FULL_LIC).Include(x => x.CourtBankruptcy)
                //    .Include(x => x.CourtInstallmentPlan)
                //    .Include(x => x.CourtExecutionInPF)
                //    .Include(x => x.CourtLitigationWork)
                //    .Include(x => x.CourtWork)
                //    .Include(x => x.CourtWriteOff)
                //    .Include(x => x.CourtStateDuty)
                //    .Include(x => x.CourtExecutionFSSP)
                //    .Include(c => c.CourtDocumentScans).FirstOrDefaultAsync();
                //return Result;
            }
        }
        public async Task AddCourtWorkRequisites(BE.Court.CourtWorkRequisites courtWorkRequisites)
        {
           using(var dbApp = new ApplicationDbContext())
            {
                dbApp.CourtWorkRequisites.Add(new DB.Model.Court.CourtWorkRequisites
                {
                    CourtGeneralInformId = courtWorkRequisites.CourtGeneralInformId,
                     Date = courtWorkRequisites.Date,
                      Number = courtWorkRequisites.Number,
                       Suma = courtWorkRequisites.Suma 
                });
                await dbApp.SaveChangesAsync();
            }
        }
        public async Task RemoveCourtWorkRequisites(int id)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                var res = await dbApp.CourtWorkRequisites.FindAsync(id);
                dbApp.CourtWorkRequisites.Remove(res);
                await dbApp.SaveChangesAsync();
            }
        }

        public async Task<List<DB.Model.Court.CourtWorkRequisites>> GetCourtWorkRequisites(int CourtGeneralInformationId)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                return await dbApp.CourtWorkRequisites.Where(x=>x.CourtGeneralInformId == CourtGeneralInformationId).ToListAsync();
            }
        }

        public async Task DeleteCourt(int Id)
        {
            using(var dbApp = new ApplicationDbContext())
            {
                var Result = await dbApp.CourtGeneralInformation.Where(x => x.Id == Id).Include(x => x.CourtBankruptcy)
                    .Include(x => x.CourtInstallmentPlan)
                    .Include(x => x.CourtExecutionInPF)
                    .Include(x => x.CourtLitigationWork)
                    .Include(x => x.CourtWork)
                    .Include(x => x.CourtWriteOff)
                    .Include(x => x.CourtStateDuty)
                    .Include(x => x.CourtExecutionFSSP)
                    .Include(x => x.CourtWorkRequisites)
                    .Include(c => c.CourtDocumentScans).FirstOrDefaultAsync();
                dbApp.CourtBankruptcy.Remove(Result.CourtBankruptcy);
                dbApp.CourtInstallmentPlan.Remove(Result.CourtInstallmentPlan);
                dbApp.CourtExecutionInPF.Remove(Result.CourtExecutionInPF);
                dbApp.CourtLitigationWork.Remove(Result.CourtLitigationWork);
                dbApp.CourtWork.Remove(Result.CourtWork);
                dbApp.CourtWriteOff.Remove(Result.CourtWriteOff);
                dbApp.CourtStateDuty.Remove(Result.CourtStateDuty);
                dbApp.CourtExecutionFSSP.Remove(Result.CourtExecutionFSSP);
                dbApp.CourtWorkRequisites.RemoveRange(Result.CourtWorkRequisites);
                dbApp.CourtCourtDocumentScans.RemoveRange(Result.CourtDocumentScans);
                dbApp.CourtGeneralInformation.Remove(Result);
                await dbApp.SaveChangesAsync();
            }
        }
        public async Task<string> saveFile(byte[] file, int CourtGeneralId, string Lic, string TypeFile, string NameFile, string User)
        {
            if (file != null)
            {
                if (!Directory.Exists($@"\\10.10.10.17\\doc_tplus_court\\{Lic}\\{CourtGeneralId}"))
                {
                    Directory.CreateDirectory($@"\\10.10.10.17\\doc_tplus_court\\{Lic}\\{CourtGeneralId}");
                }
                if (File.Exists($@"\\10.10.10.17\doc_tplus_court\\{Lic}\\{CourtGeneralId}\\{NameFile}.{TypeFile}")) return $@"Файл с название {NameFile} уже существует. Обратитесь к системному администратору!";
                File.WriteAllBytes($@"\\10.10.10.17\\doc_tplus_court\\{Lic}\\{CourtGeneralId}\\{NameFile}.{TypeFile}", file);
                using (var db = new ApplicationDbContext())
                {
                    db.CourtCourtDocumentScans.Add(new DB.Model.Court.CourtDocumentScans
                    {
                        DocumentPath = $@"{Lic}\\{CourtGeneralId}",
                        CourtDocumentScansName = $@"{NameFile}.{TypeFile}",
                        CourtGeneralInformId = CourtGeneralId,
                         Executor = User,
                          DocumentDateUpload = System.DateTime.Now,
                    });
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                return "Введите название файла";
            }
            return "Файл успешно сохранен";
        }
        public async  Task<string> DeleteFile(int Id, string User)
        {
            using (var db = new ApplicationDbContext())
            {
                var Res = db.CourtCourtDocumentScans.Where(x => x.Id == Id).FirstOrDefault();
                db.CourtCourtDocumentScans.Remove(Res);
                await db.SaveChangesAsync();
                File.Delete($@"\\10.10.10.17\\doc_tplus_court\\{Res.DocumentPath}\\{Res.CourtDocumentScansName}");
                return $"Файл {Res.CourtDocumentScansName} успешно удален";
            }
        }
        public async Task<List<DB.Model.Court.CourtDocumentScans>> GetDocumentScans(int CourtGeneralInformationId)
        {
            using(var dbApp = new ApplicationDbContext())
            {
                return await dbApp.CourtCourtDocumentScans.Where(x => x.CourtGeneralInformId == CourtGeneralInformationId).ToListAsync();
            }
        }
        public async Task<CourtDataDocumentLoad> DownLoadFile(int Id)
        {
            CourtDataDocumentLoad courtDataDocument = new CourtDataDocumentLoad();
            using (var db = new ApplicationDbContext())
            {
                var Res = await db.CourtCourtDocumentScans.Where(x => x.Id == Id).FirstOrDefaultAsync();
                courtDataDocument.FileBytes = File.ReadAllBytes($@"\\10.10.10.17\\doc_tplus_court\\{Res.DocumentPath}\\{Res.CourtDocumentScansName}");
                courtDataDocument.FileName = Res.CourtDocumentScansName;
            }
            return courtDataDocument;
        }
    }
}
