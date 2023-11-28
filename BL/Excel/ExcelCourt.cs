using AutoMapper;
using BE.Court;
using BL.Counters;
using BL.Helper;
using BL.MapperProfile;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model.Court;
using DB.Model.Court.DictiomaryModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelCourt
    {
        Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels, string User);
    }
    public class ExcelCourt : IExcelCourt
    {
        private readonly ICourt _court;
        private readonly List<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly Mapper _mapper = new CourtProfile().GetMapperBe();
        private readonly IDictionary _dictionary;
        public ExcelCourt(ICourt court, IDictionary dictionary) 
        {
            _court = court;
            _dictionary = dictionary;
            reportCourtLoadExcels = new List<ReportCourtLoadExcel>();
        }
        public async Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();

            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = new DB.Model.Court.CourtGeneralInformation();
                        CourtGeneral.CourtExecutionFSSP = new DB.Model.Court.CourtExecutionFSSP();
                        CourtGeneral.CourtExecutionInPF = new DB.Model.Court.CourtExecutionInPF();
                        CourtGeneral.CourtInstallmentPlan = new DB.Model.Court.CourtInstallmentPlan();
                        CourtGeneral.CourtBankruptcy = new DB.Model.Court.CourtBankruptcy();
                        CourtGeneral.CourtLitigationWork = new DB.Model.Court.CourtLitigationWork();
                        CourtGeneral.CourtStateDuty = new DB.Model.Court.CourtStateDuty();
                        CourtGeneral.CourtWriteOff = new DB.Model.Court.CourtWriteOff();
                        CourtGeneral.CourtWork = new DB.Model.Court.CourtWork();
                        CourtGeneral.Lic = dataRow.Cell(2).Value.TryGetLic();
                        CourtGeneral.Street = dataRow.Cell(3).Value.ToString();
                        CourtGeneral.Home = dataRow.Cell(4).Value.ToString();
                        CourtGeneral.Flat = dataRow.Cell(5).Value.ToString();
                        CourtGeneral.FioDuty = $"{dataRow.Cell(6).Value.ToString()} {dataRow.Cell(7).Value.ToString()} {dataRow.Cell(8).Value.ToString()}";
                        CourtGeneral.CourtWork.FioSendCourt = dataRow.Cell(9).Value.ToString();
                        CourtGeneral.CourtWork.DateTask = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtBegin = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());
                        CourtGeneral.CourtWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());
                        CourtGeneral.ShareOfOwnership = dataRow.Cell(14).Value.ToString();
                        if (dictionaryCourt.FirstOrDefault(x => x.Id == 21).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.ShareOfOwnership) == null)
                            exceptions.Append("Доля собственности не найдена в справочнике");
                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }

                        var result = await _court.CreateCourtExcel(CourtGeneral, User);

                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Id = $"СДПФ-{result.Id}", Description = "Успешно создано дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            return CreateResultCourtLoader(reportCourtLoadExcels);
        }
        private DataTable CreateResultCourtLoader(List<ReportCourtLoadExcel> reportCourtLoadExcels)
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Индификатор судебного дела"),new DataColumn("Строка"),
                                        new DataColumn("Описание")});
            foreach (var Item in reportCourtLoadExcels)
                dt.Rows.Add(Item.Id, Item.Line, Item.Description);

            return dt;
        }
    }
}
