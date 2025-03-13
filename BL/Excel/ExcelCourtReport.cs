using AutoMapper;
using BE.Court;
using BE.Service;
using BL.Helper;
using BL.MapperProfile;
using BL.Notification;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelCourtReport
    {
        Task<DataTable> ReestyGPAccountingDepartment(XLWorkbook Excels, string User, CourtTypeReport courtTypeReport, DateTime dateTime);
        Task<XLWorkbook> WriteOff(XLWorkbook Excels, string User, CourtTypeReport2 courtTypeReport, DateTime dateTime);
    }
    public class ExcelCourtReport : IExcelCourtReport
    {
        private readonly ICourt _court;
        private readonly List<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly IDictionary _dictionary;
        private readonly INotificationMail _notificationMail;
        private readonly IFlagsAction _flagsAction;
        private readonly IPersonalData _personalData;
        public ExcelCourtReport(ICourt court, IDictionary dictionary, INotificationMail notificationMail, IFlagsAction flagsAction, IPersonalData personalData)
        {
            _court = court;
            _dictionary = dictionary;
            reportCourtLoadExcels = new List<ReportCourtLoadExcel>();
            _notificationMail = notificationMail;
            _flagsAction = flagsAction;
            _personalData = personalData;
        }
        public async Task<DataTable> ReestyGPAccountingDepartment(XLWorkbook Excels, string User, CourtTypeReport courtTypeReport, DateTime dateTime)
        {
            var date = new DateTime();
            var courtGeneralInformations = new List<DB.Model.Court.CourtGeneralInformation>();
            var startDate = _flagsAction.GetFlag(EnumFlags.ReestyGPAccountingDepartment).DateTime.Value;
            if(dateTime < startDate)
            {
                date = startDate;
                startDate = dateTime;
                dateTime = date;
            }
            else
            {
                date = dateTime;
            }

            courtGeneralInformations.AddRange(await _court.GetCourtWithFilter(x => x.CourtWork.DateAccountingDepartment.HasValue && x.CourtWork.DateAccountingDepartment >= startDate 
            && x.CourtWork.DateAccountingDepartment <= dateTime));

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
            foreach(var item in courtGeneralInformations)
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
                {   if(flag.DateTime < date)
                        flag.DateTime = date;
                }
                context.SaveChanges();
            }
            return dt;
        }

        public async Task<XLWorkbook> WriteOff(XLWorkbook Excels, string User, CourtTypeReport2 courtTypeReport, DateTime dateTime)
        {
            var worksheet = Excels.Worksheets.Add("Лист1");
            using (var context = new ApplicationDbContext())
            {
                var courtsSearchWriteOff = await context.CourtWriteOff.Where(x =>
                    x.DocumentsPreparedWriteOff != null ||
                    x.DateWriteOffEnd != null || 
                    x.DateWriteOffBegin != null || 
                    x.WriteOffStatus != null ||
                    x.DateWriteOff != null ||
                    x.Comment != null ||
                    x.SumOd != null ||
                    x.SumPeny != null ||
                    x.SumGp != null ||
                    x.ReasonWriteOff != null ||
                    x.SubjectWriteOff != null
                    ).Include(x => x.CourtGeneralInformation).Select(x=>new { x.CourtGeneralInformation.Id, x.CourtGeneralInformation.Lic }).ToListAsync();
                var ids = courtsSearchWriteOff.Select(x => x.Id).ToList();
                var courts = await _court.GetCourtWithFilter(x => ids.Contains(x.Id));

                var lics = courtsSearchWriteOff.Select(x => x.Lic).ToList();
                var persDatas = await context.PersonalInformation.Where(x => lics.Contains(x.full_lic)).ToListAsync();

                #region Columns
                worksheet.SetValue(1, 1, "Лицевой счет");
                worksheet.SetValue(1, 2, "Статус лицевого счета");
                worksheet.SetValue(1, 3, "Фамилия");
                worksheet.SetValue(1, 4, "Имя");
                worksheet.SetValue(1, 5, "Отчество");
                worksheet.SetValue(1, 6, "Улица");
                worksheet.SetValue(1, 7, "Дом");
                worksheet.SetValue(1, 8, "Квартира");
                worksheet.SetValue(1, 9, "Долг на текущую дату");
                worksheet.SetValue(1, 10, "№ карточки дела");
                worksheet.SetValue(1, 11, "Фамилия должника");
                worksheet.SetValue(1, 12, "Имя должника");
                worksheet.SetValue(1, 13, "Отчество должника");
                worksheet.SetValue(1, 14, "Дата рождения");
                worksheet.SetValue(1, 15, "Дата смерти");
                worksheet.SetValue(1, 16, "Документы подготовлены к списанию (да/нет):");
                worksheet.SetValue(1, 17, "Сумма списания всего:");
                worksheet.SetValue(1, 18, "Сумма списания ГП:");
                worksheet.SetValue(1, 19, "Сумма списания пени:");
                worksheet.SetValue(1, 20, "Сумма списания ОД:");
                worksheet.SetValue(1, 21, "Начальный период списания:");
                worksheet.SetValue(1, 22, "Конечный период списания:");
                worksheet.SetValue(1, 23, "Статус списания:");
                worksheet.SetValue(1, 24, "Дата списания:");
                worksheet.SetValue(1, 25, "Основания списания:");
                worksheet.SetValue(1, 26, "Субьект списания:");
                worksheet.SetValue(1, 27, "Примечание:");
                worksheet.SetValue(1, 28, "ФИО сотрудника (направившего дело в суд):");
                worksheet.SetValue(1, 29, "Дата задания");
                worksheet.SetValue(1, 30, "Сумма ОД и пени, предъявленная в суд");
                worksheet.SetValue(1, 31, "Сумма ОД, предъявленная в суд");
                worksheet.SetValue(1, 32, "Сумма пени, предъявленная в суд");
                worksheet.SetValue(1, 33, "Период задолженности начальный");
                worksheet.SetValue(1, 34, "Период задолженности конечный");
                worksheet.SetValue(1, 35, "Сумма ГП (указанная в судебном приказе)");
                worksheet.SetValue(1, 36, "Дата принятия заявления судом");
                worksheet.SetValue(1, 37, "№ СП");
                worksheet.SetValue(1, 38, "Дата СП");
                worksheet.SetValue(1, 39, "ПРИМЕЧАНИЕ");
                #endregion

                var debts = _personalData.GetDebtInfosForLics(courts.Select(x => x.Lic).ToList());

                var notes = new ConcurrentBag<CourtNotesInfo>();

                Parallel.ForEach(courts, (item) =>
                {
                    notes.Add( new CourtNotesInfo
                    {
                        CourtId = item.Id,
                        Lic = item.Lic,
                        Note = _court.GetNote(item.Id, item.Lic)
                    });
                });


                foreach (var item in courts.Select((value, i) => new { i, value }))
                {
                    var pers = persDatas.FirstOrDefault(x => x.full_lic == item.value.Lic);
                    worksheet.SetValue(item.i + 2, 1, item.value.Lic);
                    worksheet.SetValue(item.i + 2, 2, item.value.StatusCard);
                    worksheet.SetValue(item.i + 2, 3, pers?.LastName);
                    worksheet.SetValue(item.i + 2, 4, pers?.FirstName);
                    worksheet.SetValue(item.i + 2, 5, pers?.MiddleName);
                    worksheet.SetValue(item.i + 2, 6, pers?.Street);
                    worksheet.SetValue(item.i + 2, 7, pers?.House);
                    worksheet.SetDataType(item.i + 2, 8, XLDataType.Text).SetValue(pers?.Flat);
                    var debt = debts.FirstOrDefault(x => x.Lic == item.value.Lic);
                    worksheet.SetValue(item.i + 2, 9, debt?.CurrentDebt);
                    worksheet.SetValue(item.i + 2, 10, item.value.Id);
                    worksheet.SetValue(item.i + 2, 11, item.value.LastName);
                    worksheet.SetValue(item.i + 2, 12, item.value.FirstName);
                    worksheet.SetValue(item.i + 2, 13, item.value.Surname);
                    worksheet.SetValue(item.i + 2, 14, item.value.DateBirthday);
                    worksheet.SetValue(item.i + 2, 15, item.value.DateDeath);
                    worksheet.SetValue(item.i + 2, 16, item.value.CourtWriteOff.DocumentsPreparedWriteOff);
                    worksheet.SetValue(item.i + 2, 17, item.value.CourtWriteOff.SumWriteOff);
                    worksheet.SetValue(item.i + 2, 18, item.value.CourtWriteOff.SumGp);
                    worksheet.SetValue(item.i + 2, 19, item.value.CourtWriteOff.SumPeny);
                    worksheet.SetValue(item.i + 2, 20, item.value.CourtWriteOff.SumOd);
                    worksheet.SetValue(item.i + 2, 21, item.value.CourtWriteOff.DateWriteOffBegin);
                    worksheet.SetValue(item.i + 2, 22, item.value.CourtWriteOff.DateWriteOffEnd);
                    worksheet.SetValue(item.i + 2, 23, item.value.CourtWriteOff.WriteOffStatus);
                    worksheet.SetValue(item.i + 2, 24, item.value.CourtWriteOff.DateWriteOff);
                    worksheet.SetValue(item.i + 2, 25, item.value.CourtWriteOff.ReasonWriteOff);
                    worksheet.SetValue(item.i + 2, 26, item.value.CourtWriteOff.SubjectWriteOff);
                    worksheet.SetValue(item.i + 2, 27, item.value.CourtWriteOff.Comment);
                    worksheet.SetValue(item.i + 2, 28, item.value.CourtWork.FioSendCourt);
                    worksheet.SetValue(item.i + 2, 29, item.value.CourtWork.DateTask);
                    worksheet.SetValue(item.i + 2, 30, item.value.CourtWork.AmountdebtTransferredToCourtTotal);
                    worksheet.SetValue(item.i + 2, 31, item.value.CourtWork.SumOdSendCourt);
                    worksheet.SetValue(item.i + 2, 32, item.value.CourtWork.SumPenySendCourt);
                    worksheet.SetValue(item.i + 2, 33, item.value.CourtWork.PeriodDebtBegin);
                    worksheet.SetValue(item.i + 2, 34, item.value.CourtWork.PeriodDebtEnd);
                    worksheet.SetValue(item.i + 2, 35, item.value.CourtWork.SumGP);
                    worksheet.SetValue(item.i + 2, 36, item.value.CourtWork.DateReceptionCourt);
                    worksheet.SetValue(item.i + 2, 37, item.value.CourtWork.NumberSP);
                    worksheet.SetValue(item.i + 2, 38, item.value.CourtWork.DateSP);
                    var note = notes.FirstOrDefault(x=>x.Lic == item.value.Lic && x.CourtId == item.value.Id);
                    worksheet.SetValue(item.i + 2, 39, note.Note);
                }
                worksheet.Columns().AdjustToContents();
                return Excels;
            }
        }
       
    }
}
