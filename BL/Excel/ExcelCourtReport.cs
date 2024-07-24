using AutoMapper;
using BE.Court;
using BE.Service;
using BL.MapperProfile;
using BL.Notification;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
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
            DataTable dt = new DataTable("Report");
            var dataColumn = new DataColumn[]
            {
                new DataColumn("#"),
                new DataColumn("БЕ/ наименование филиала"),
                new DataColumn("Наименование контрагента"),
                new DataColumn("ИНН контрагента"),
                new DataColumn("Номер решения суда СП"),
                new DataColumn("Дата вступления в законную силу"),
                new DataColumn("Номер/ Дата испол листа СП"),
                new DataColumn("Дата документа"),
                new DataColumn("№ договора"),
                new DataColumn("Код FA договора"),
                new DataColumn("Наименование FA договора"),
                new DataColumn("Сумма основного долга"),
                new DataColumn("Сумма процентов, пени, штрафов"),
                new DataColumn("Сумма гос пошлины"),
                new DataColumn("Место возникновения прибыли (Город/площадка)"),
                new DataColumn("ФИО ответственного сотрудника в РЦПО"),
                new DataColumn("Адрес"),
                new DataColumn("ФИО сотрудника, направившего дело в суд"),
                new DataColumn("Номер карточки")
            };
            dt.Columns.AddRange(dataColumn);
            int Number = 1;
            foreach(var item in CourtGeneralInformation)
            {
                dt.Rows.Add(
                    Number,///#
                    "Пензенский",///БЕ/ наименование филиала
                    $"{item.LastName} {item.FirstName} {item.Surname}",///Наименование контрагента
                    "",///ИНН контрагента
                    "",///Номер решения суда СП
                    item.CourtWork.DateSP?.AddDays(11).ToString("dd.MM.yyyy"),///Дата вступления в законную силу
                    item.CourtWork.NumberSP,///Номер/ Дата испол листа СП
                    item.CourtWork.DateSP?.ToString("dd.MM.yyyy"),///Дата документа
                    item.Lic,///№ договора
                    "FA057",///Код FA договора
                    "",///Наименование FA договора
                    item.CourtWork.SumOdSendCourt,///Сумма основного долга
                    "",///Сумма процентов, пени, штрафов
                    item.CourtWork.SumGP,///Сумма гос пошлины
                    "Пенза/7W00",///Место возникновения прибыли (Город/площадка)
                    "Тарасова Е.И",///ФИО ответственного сотрудника в РЦПО
                    item.Street, ///Адрес
                    item.CourtWork.FioSendCourt ///ФИО сотрудника, направившего дело в суд
                    ,$"П-{item.Id}"
                    );
                Number++;
            }
            _notificationMail.SendEmailResultLoadCourt(dt, $"{courtTypeReport.GetDescription()} - {dateTime.ToString("dd-MM-yyyy")}.xlsx");
            using(var context = new ApplicationDbContext())
            {
                var flag = context.Flags.FirstOrDefault(x=>x.NameAction == nameof(EnumFlags.ReestyGPAccountingDepartment));
                if (flag != null)
                {
                    flag.DateTime = DateTime.Now;
                }
                context.SaveChanges();
            }
            return dt;
        }
    }
}
