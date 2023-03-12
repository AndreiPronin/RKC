using AppCache;
using BE.ApiT_;
using BE.DPU;
using BE.PersData;
using BL.Excel;
using BL.Extention;
using DB.DataBase;
using DB.Model;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using NaturalSort;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IDpu
    {
        Task<List<SearchAutocompleteResultDPU>> SearchAutocompleteDPU(string Text);
        Task<DPUHelpCalculationInstallation> GetDpu(int id);
        Task<List<DPUSummaryHouses>> GetDPUSummaryHouses();
        Task<List<DPUHelpCalculationInstallation>> GetWatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
        Task<DpuDataDocumentLoad> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
        Task DpuSaveNote(int id, string Text);   
    }
    public class Dpu : IDpu
    {
        public static readonly object LockObject = new object();
        private readonly ICacheApp _cacheApp;
        public Dpu(ICacheApp cacheApp)
        {
            _cacheApp = cacheApp;
        }
        public async Task<List<SearchAutocompleteResultDPU>> SearchAutocompleteDPU(string Text)
        {
            List<SearchAutocompleteResultDPU> searchAutocompleteResultDPU = new List<SearchAutocompleteResultDPU>();
            var Words = Text.Split(' ');
           
            using (var AppDb = new ApplicationDbContext())
            {
                var datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(DPUHelpCalculationInstallation));
                if (datas == null)
                {
                    lock (LockObject) 
                    {
                        datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(DPUHelpCalculationInstallation));
                        if (datas == null)
                        {
                            var DataTable = AppDb.dPUHelpCalculationInstallations.Select(x => new SerachAutoCompleteModel
                            {
                                Id = x.Id,
                                Street = x.Street.Trim().ToLower(),
                                Home = x.Home.Trim().ToLower(),
                                Cadr = x.Cadr.Trim().ToLower(),
                                Flat = x.Flat.Trim().ToLower(),
                                FullName = x.FullName.Trim().ToLower(),
                                NewFullLic = x.NewFullLic.Trim().ToLower(),
                                Period = x.Period,
                            }).ToList();
                            var query = DataTable.GroupBy(d => new { d.Street, d.Home, d.Flat })
                            .SelectMany(g => g.OrderByDescending(d => d.Period)
                                              .Take(1)).ToList();
                            var querys = DataTable.GroupBy(d => new { d.Street, d.Home, d.Flat })
                            .SelectMany(g => g.OrderByDescending(d => d.Period)
                                              .Take(1)).ToList();
                            _cacheApp.SetValue<List<SerachAutoCompleteModel>>(nameof(DPUHelpCalculationInstallation), query);
                            datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(DPUHelpCalculationInstallation));
                        }
                    }
                }
                var Dates = AppDb.dPUHelpCalculationInstallations.OrderByDescending(x=>x.Period).Select(x=>x.Period).FirstOrDefault();
                List<SerachAutoCompleteModel> dPUHelps = datas ;
                foreach (var Item in Words)
                {
                    var Word = Item.Trim().ToLower();
                    var Date = DateTime.TryParse(Word, out var date);
                    //if (Date)
                    //{
                    //    dPUHelps = dPUHelps.Where(x => x.Period.Value.Month == date.Month && x.Period.Value.Year == date.Year);
                    //    continue;
                    //}
                    //else
                    //{
                    //    dPUHelps = dPUHelps.Where(x => x.Period.Value.Month >= Dates.Value.Month && x.Period.Value.Year >= Dates.Value.Year);
                    //}
                    if (Word.Contains("кв."))
                    {
                        Word = Word.Replace("кв.", "");
                        dPUHelps = dPUHelps.Where(x => x.Flat == Word).ToList();
                        continue;
                    }
                    if (Word.Contains("ул."))
                    {
                        Word = Word.Replace("ул.", "");
                        dPUHelps = dPUHelps.Where(x => x.Street.Contains(Word)).ToList();
                        continue;
                    }
                    if (Word.Contains("д."))
                    {
                        Word = Word.Replace("д.", "");
                        dPUHelps = dPUHelps.Where(x => x.Home == Word).ToList();
                        continue;
                    }

                    dPUHelps = dPUHelps.Where(x => x.Street.Contains(Word) || x.Home.Contains(Word) || x.Cadr.Contains(Word)
                        || x.Flat.Contains(Word)  || x.FullName.Contains(Word)
                        || x.NewFullLic.Contains(Word)).ToList();
                }
                var Result =  dPUHelps.Take(100).ToList();
                var res = Result.OrderBy(n => n.Street, comparer: new NaturalSortComparer()).ToList();
                foreach (var Item in res)
                {
                    searchAutocompleteResultDPU.Add(new SearchAutocompleteResultDPU
                    {
                        Id = Item.Id,
                        Value = $"{Item.Cadr} ул.{Item.Street} д.{Item.Home} кв. {Item.Flat}  л.счет.{Item.NewFullLic} ФИО.{Item.FullName}"
                    });
                }
            }
            return searchAutocompleteResultDPU;
        }
        public async Task<DPUHelpCalculationInstallation> GetDpu(int id)
        {
            using (var AppDb = new ApplicationDbContext())
            {
                return await AppDb.dPUHelpCalculationInstallations.FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task<List<DPUSummaryHouses>> GetDPUSummaryHouses()
        {
            using (var AppDb = new ApplicationDbContext())
            {
                return await AppDb.dPUSummaryHouses.OrderBy(x=>x.Street).ToListAsync();
            }
        }

        public async Task DpuSaveNote(int id, string Text)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                var dpu = await dbApp.dPUHelpCalculationInstallations.FirstOrDefaultAsync(x => x.Id == id);
                dpu.Note = Text;
                await dbApp.SaveChangesAsync();
            }
        }

        public async Task<List<DPUHelpCalculationInstallation>> GetWatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                return await dbApp.dPUHelpCalculationInstallations.Where(x=>x.Period.Value >= DateFrom && x.Period.Value<= DateTo && x.NewFullLic == FullLic).ToListAsync();
            }
        }

        public async Task<DpuDataDocumentLoad> DownLoadHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {

            DpuDataDocumentLoad dpuDataDocument = new DpuDataDocumentLoad();
            dpuDataDocument.FileName = $@"Справка расчета {FullLic}.xlsx";
            var dateFrom = Convert.ToDateTime(DateFrom.ToString("yyyy,MM"));
            var dateTo = Convert.ToDateTime(DateTo.ToString("yyyy,MM")).AddMonths(1);
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.dPUHelpCalculationInstallations.Where(x => x.NewFullLic == FullLic && x.Period >= dateFrom && x.Period <= dateTo).ToListAsync();
                dpuDataDocument.FileBytes = ExcelHelpСalculation.Generate(Result);
            }
            return dpuDataDocument;
        }
    }
}
