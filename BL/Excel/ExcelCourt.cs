using AutoMapper;
using BE.Court;
using BL.Counters;
using BL.Helper;
using BL.MapperProfile;
using BL.Services;
using ClosedXML.Excel;
using DB.Model.Court;
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
        private readonly ConcurrentBag<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly Mapper mapper = new CourtProfile().GetMapperBe();
        public ExcelCourt(ICourt court) 
        {
            _court = court;
            reportCourtLoadExcels = new ConcurrentBag<ReportCourtLoadExcel>();
        }
        public async Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels, string User)
        {
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();
            //foreach (var dataRow in nonEmptyDataRows)
            //{
            //    if (dataRow.RowNumber() > 1)
            //    {
            //        try
            //        {
            //            var Lic = dataRow.Cell(2).Value.TryGetLic();
            //            var Court = await _court.CreateCourtExcel(Lic, DateTime.Now.ToString(),User);
            //            var CourtGeneral = mapper.Map<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>(Court);
            //            CourtGeneral.Street = dataRow.Cell(3).Value.ToString();
            //            CourtGeneral.Home = dataRow.Cell(4).Value.ToString();
            //            CourtGeneral.Flat = dataRow.Cell(5).Value.ToString();
            //            CourtGeneral.FioDuty = $"{dataRow.Cell(6).Value.ToString()} {dataRow.Cell(7).Value.ToString()} {dataRow.Cell(8).Value.ToString()}";

            //            CourtGeneral.CourtWork.FioSendCourt = dataRow.Cell(9).Value.ToString();
            //            CourtGeneral.CourtWork.DateTask = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());
            //            CourtGeneral.CourtWork.PeriodDebtBegin = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());
            //            CourtGeneral.CourtWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());
            //            CourtGeneral.CourtWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());

            //            await _court.SaveCourt(CourtGeneral, User);
            //            reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Id=$"СДПФ-{Court.Id}", Description = "Успешно создано дело" });
            //            break;
            //        }
            //        catch(Exception ex)
            //        {
            //            reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Line = dataRow.RowNumber().ToString(), Description = ex.Message });
            //        }
            //    }
            //}
            Parallel.ForEach(nonEmptyDataRows, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, async dataRow =>
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        var Lic = dataRow.Cell(2).Value.TryGetLic();
                        var Court = await _court.CreateCourtExcel(Lic, DateTime.Now.ToString(), User);
                        var CourtGeneral = mapper.Map<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>(Court);
                        CourtGeneral.Street = dataRow.Cell(3).Value.ToString();
                        CourtGeneral.Home = dataRow.Cell(4).Value.ToString();
                        CourtGeneral.Flat = dataRow.Cell(5).Value.ToString();
                        CourtGeneral.FioDuty = $"{dataRow.Cell(6).Value.ToString()} {dataRow.Cell(7).Value.ToString()} {dataRow.Cell(8).Value.ToString()}";

                        CourtGeneral.CourtWork.FioSendCourt = dataRow.Cell(9).Value.ToString();
                        CourtGeneral.CourtWork.DateTask = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtBegin = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());
                        CourtGeneral.CourtWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());

                        await _court.SaveCourt(CourtGeneral, User);
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Id = $"СДПФ-{Court.Id}", Description = "Успешно создано дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            });
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Индификатор судебного дела"),new DataColumn("Строка"),
                                        new DataColumn("Описание")});
            foreach(var Item in reportCourtLoadExcels)
                dt.Rows.Add(Item.Id,Item.Line, Item.Description);
            return dt;
        }
    }
}
