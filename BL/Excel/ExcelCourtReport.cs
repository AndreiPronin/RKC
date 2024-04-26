using AutoMapper;
using BE.Court;
using BL.MapperProfile;
using BL.Notification;
using BL.Services;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelCourtReport
    {
        Task<DataTable> ReestyGPAccountingDepartment(XLWorkbook Excels, string User, CourtTypeReport courtTypeReport, DateTime dateTime);
    }
    public class ExcelCourtReport : IExcelCourtReport
    {
        private readonly ICourt _court;
        private readonly List<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly IDictionary _dictionary;
        private readonly INotificationMail _notificationMail;
        public ExcelCourtReport(ICourt court, IDictionary dictionary, INotificationMail notificationMail)
        {
            _court = court;
            _dictionary = dictionary;
            reportCourtLoadExcels = new List<ReportCourtLoadExcel>();
            _notificationMail = notificationMail;
        }
        public async Task<DataTable> ReestyGPAccountingDepartment(XLWorkbook Excels, string User, CourtTypeReport courtTypeReport, DateTime dateTime)
        {
            var CourtGeneralInformation = await _court.GetCourtWithFilter(x=>x.CourtWork.DateAccountingDepartment == dateTime);
            
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, $"{courtTypeReport.GetDescription()} - {dateTime.ToString("dd-MM-yyyy")}.xlsx");
            return results;
        }
        private DataTable CreateResultCourtLoader(List<ReportCourtLoadExcel> reportCourtLoadExcels)
        {
            DataTable dt = new DataTable("Report");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Уникальный переданный номер из загрузочного файла"), new DataColumn("Индификатор судебного дела"),new DataColumn("Строка"),
                                        new DataColumn("Описание")});
            foreach (var Item in reportCourtLoadExcels)
                dt.Rows.Add(Item.IdCourt, Item.Id, Item.Line, Item.Description);

            return dt;
        }
    }
}
