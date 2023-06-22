using AppCache;
using BE.ApiT_;
using BE.DPU;
using BE.PersData;
using BL.Excel;
using BL.Extention;
using DB.DataBase;
using DB.Model;
using DB.Query;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
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
        Task<DPUHelpCalculationInstallationView> GetDpu(int id);
        Task<List<DPUSummaryHousesView>> GetDPUSummaryHouses();
        Task<List<DPUHelpCalculationInstallationView>> GetWatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo);
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
            await Task.CompletedTask;
            using (var AppDb = new ApplicationDbContext())
            {
                var datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(SerachAutoCompleteModel));
                if (datas == null)
                {
                    lock (LockObject) 
                    {
                        datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(SerachAutoCompleteModel));
                        if (datas == null)
                        {
                            var result = AppDb.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).ToList();
                            var DataTable = result.Select(x => new SerachAutoCompleteModel
                            {
                                Id = x.Id,
                                Street = x.Street?.Trim().ToLower(),
                                Home = x.Home?.Trim().ToLower(),
                                Cadr = x.Cadr?.Trim().ToLower(),
                                Flat = x.Flat?.Trim().ToLower(),
                                FullName = x.FullName?.Trim().ToLower(),
                                NewFullLic = x.NewFullLic?.Trim().ToLower(),
                                Period = x.Period,
                            }).ToList();
                            var query = DataTable.GroupBy(d => new { d.Street, d.Home, d.Flat, d.NewFullLic })
                            .SelectMany(g => g.OrderByDescending(d => d.Period)
                                              .Take(1)).ToList();
                            var querys = DataTable.GroupBy(d => new { d.Street, d.Home, d.Flat, d.NewFullLic })
                            .SelectMany(g => g.OrderByDescending(d => d.Period)
                                              .Take(1)).ToList();
                            foreach (var Item in query)
                            {
                                Item.Street = Item.Street is null ? "" : Item.Street;
                                Item.Home = Item.Home is null ? "" : Item.Home;
                                Item.Cadr = Item.Cadr is null ? "" : Item.Cadr;
                                Item.Flat = Item.Flat is null ? "" : Item.Flat;
                                Item.FullName = Item.FullName is null ? "" : Item.FullName;
                                Item.NewFullLic = Item.NewFullLic is null ? "" : Item.NewFullLic;
                            }
                            _cacheApp.SetValue<List<SerachAutoCompleteModel>>(nameof(SerachAutoCompleteModel), query);
                            datas = _cacheApp.GetValue<List<SerachAutoCompleteModel>>(nameof(SerachAutoCompleteModel));
                        }
                    }
                }
                var Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                if (Datas == null)
                {
                    lock (LockObject)
                    {
                        Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                        if (Datas == null)
                        {
                            var query = AppDb.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).ToList();
                            foreach(var Item in query)
                            {
                                Item.Street = Item.Street is null ? "" : Item.Street;
                                Item.Home = Item.Home is null ? "" : Item.Home;
                                Item.Cadr = Item.Cadr is null ? "" : Item.Cadr;
                                Item.Flat = Item.Flat is null ? "" : Item.Flat;
                                Item.FullName = Item.FullName is null ? "" : Item.FullName;
                                Item.NewFullLic = Item.NewFullLic is null ? "" : Item.NewFullLic;
                            }
                            _cacheApp.SetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView), query);
                            Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                        }
                    }
                }
                var Dates = Datas.FirstOrDefault();
                List<SerachAutoCompleteModel> dPUHelps = datas ;
                foreach (var Item in Words)
                {
                    var Word = Item.Trim().ToLower();
                    var Date = DateTime.TryParse(Word, out var date);
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
        public async Task<DPUHelpCalculationInstallationView> GetDpu(int id)
        {
            using (var AppDb = new ApplicationDbContext())
            {
                var Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                if (Datas == null)
                {
                    lock (LockObject)
                    {
                        Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                        if (Datas == null)
                        {
                            var query = AppDb.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).ToList();
                            foreach (var Item in query)
                            {
                                Item.Street = Item.Street is null ? "" : Item.Street;
                                Item.Home = Item.Home is null ? "" : Item.Home;
                                Item.Cadr = Item.Cadr is null ? "" : Item.Cadr;
                                Item.Flat = Item.Flat is null ? "" : Item.Flat;
                                Item.FullName = Item.FullName is null ? "" : Item.FullName;
                                Item.NewFullLic = Item.NewFullLic is null ? "" : Item.NewFullLic;
                            }
                            _cacheApp.SetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView), query);
                            Datas = _cacheApp.GetValue<List<DPUHelpCalculationInstallationView>>(nameof(DPUHelpCalculationInstallationView));
                        }
                    }
                }
                await Task.CompletedTask;
                var result = Datas.FirstOrDefault(x => x.Id == id);
                var dPUSummaryHouses = await GetDPUSummaryHouses();
                result.Period = dPUSummaryHouses.FirstOrDefault(x=>x.Cadr == result.Cadr).PeriodExhibid.Value;
                return result;
            }
        }
        public async Task<List<DPUSummaryHousesView>> GetDPUSummaryHouses()
        {
            using (var AppDb = new ApplicationDbContext())
            {
                var ttt = AppDb.Database.SqlQuery<DPUSummaryHousesView>(QueryDpu.SqlDPUSummaryHousesView).ToList();
                await Task.CompletedTask;
                return ttt.OrderBy(x=>x.Street).ToList();
            }
        }

        public async Task DpuSaveNote(int id, string Text)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                var dpu = await dbApp.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).FirstOrDefaultAsync(x => x.Id == id);
                dpu.Note = Text;
                await dbApp.SaveChangesAsync();
            }
        }

        public async Task<List<DPUHelpCalculationInstallationView>> GetWatchHelpСalculation(string FullLic, DateTime DateFrom, DateTime DateTo)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                var result = dbApp.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).ToList();
                await Task.CompletedTask;
                var results = result.Where(x => x.Period >= DateFrom && x.Period <= DateTo && x.NewFullLic == FullLic).ToList();
                foreach(var Item in results)
                {
                    Item.PercentageRate = Item.PercentageRate is null ? 0 : Item.PercentageRate.Value;
                    Item.AccruedMainPayment = Item.AccruedMainPayment is null ? 0 : Item.AccruedMainPayment.Value;
                    Item.AccruedPercentage = Item.AccruedPercentage is null ? 0 : Item.AccruedPercentage.Value;
                    Item.TotalAccrued = Item.TotalAccrued is null ? 0 : Item.TotalAccrued.Value;
                    Item.PaymentMainDebt = Item.PaymentMainDebt is null ? 0 : Item.PaymentMainDebt.Value;
                    Item.PercentagePayment = Item.PercentagePayment is null ? 0 : Item.PercentagePayment.Value;
                    Item.Paid = Item.Paid is null ? 0 : Item.Paid.Value;
                    Item.ToPay = Item.ToPay is null ? 0 : Item.ToPay.Value;
                    Item.SaldoEndPeriodDebt = Item.SaldoEndPeriodDebt is null ? 0 : Item.SaldoEndPeriodDebt.Value;
                    Item.SaldoEndPeriodPercentage = Item.SaldoEndPeriodPercentage is null ? 0 : Item.SaldoEndPeriodPercentage.Value;
                }
                return results;
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
                var res = db.Database.SqlQuery<DPUHelpCalculationInstallationView>(QueryDpu.SqlDPUHelpCalcuLationInstallationView).ToList();
                await Task.CompletedTask;
                var Result = res.Where(x => x.NewFullLic == FullLic && x.Period >= dateFrom && x.Period <= dateTo).ToList();
                foreach (var Item in Result)
                {
                    Item.PercentageRate = Item.PercentageRate is null ? 0 : Item.PercentageRate.Value;
                    Item.AccruedMainPayment = Item.AccruedMainPayment is null ? 0 : Item.AccruedMainPayment.Value;
                    Item.AccruedPercentage = Item.AccruedPercentage is null ? 0 : Item.AccruedPercentage.Value;
                    Item.TotalAccrued = Item.TotalAccrued is null ? 0 : Item.TotalAccrued.Value;
                    Item.PaymentMainDebt = Item.PaymentMainDebt is null ? 0 : Item.PaymentMainDebt.Value;
                    Item.PercentagePayment = Item.PercentagePayment is null ? 0 : Item.PercentagePayment.Value;
                    Item.Paid = Item.Paid is null ? 0 : Item.Paid.Value;
                    Item.ToPay = Item.ToPay is null ? 0 : Item.ToPay.Value;
                    Item.SaldoEndPeriodDebt = Item.SaldoEndPeriodDebt is null ? 0 : Item.SaldoEndPeriodDebt.Value;
                    Item.SaldoEndPeriodPercentage = Item.SaldoEndPeriodPercentage is null ? 0 : Item.SaldoEndPeriodPercentage.Value;
                }
                dpuDataDocument.FileBytes = ExcelHelpСalculation.Generate(Result);
            }
            return dpuDataDocument;
        }
    }
}
