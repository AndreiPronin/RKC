using AutoMapper;
using BE.Court;
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
        Task<int> SaveCourt(BE.Court.CourtGeneralInformation courtGeneralInformation);
        Task<List<CourtGeneralInformation>> GetAllCourtFullLic(string FullLic);
    }
    public class Court : ICourt
    {
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
                    .Include(c => c.CourtDocumentScans).FirstOrDefaultAsync();
                return Result;
            }
        }

        public async Task<int> SaveCourt(BE.Court.CourtGeneralInformation courtGeneralInformation)
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
        public async Task<int> CreateCourt(string FullLic,string NumberIP)
        {
            using (var db = new ApplicationDbContext())
            {
                var Model = new CourtGeneralInformation { Lic = FullLic };
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
                db.CourtExecutionFSSP.Add(new DB.Model.Court.CourtExecutionFSSP { CourtGeneralInformationId = Id, NumberIP = NumberIP });
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
                        query = query.Where(x => x.Home == searchModel.Home);
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
    }
}
