using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtLitigationWork
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// Дата определения об отмене СП
        /// </summary>
        public DateTime? DateDecisionCansel { get; set; }
        /// <summary>
        /// Дата получения определения об отмене СП
        /// </summary>
        public DateTime? DateReceipt { get; set; }
        /// <summary>
        /// Дата передачи искового заявления в суд
        /// </summary>
        public DateTime? DateSubmission { get; set; }
        /// <summary>
        /// Дата передачи документов в ПИР РЦПО
        /// </summary>
        public DateTime? DateSendPirRCO { get; set; }
        /// <summary>
        /// Взысканная сумма - всего
        /// </summary>
        public double? AmountWithdrawnAll { get; set; }
        /// <summary>
        /// Взысканная сумма - ОД
        /// </summary>
        public double? AmountWithdrawnOd { get; set; }
        /// <summary>
        /// Взысканная сумма - пени
        /// </summary>
        public double? AmountWithdrawnPeny { get; set; }
        /// <summary>
        /// Взысканная сумма - ГП
        /// </summary>
        public double? AmountWithdrawnGp { get; set; }
        /// <summary>
        /// Наименование суда
        /// </summary>
        public string NameCourt { get; set; }
        /// <summary>
        /// Адресс суда
        /// </summary>
        public string AddressCourt { get; set; }
        /// <summary>
        /// ФИО сотрудника (направившего дело в суд)
        /// </summary>
        public string FioSendCourt { get; set; }
        /// <summary>
        /// Способ отправки заявления в суд
        /// </summary>
        public string HowSubmitApplicationCourt { get; set; }
        /// <summary>
        /// Cумма иска - всего
        /// </summary>
        public double? SumIskaAll { get; set; }
        /// <summary>
        /// Сумма ОД предъявленная в суд
        /// </summary>
        public double? SumOdSendCourt { get; set; }
        /// <summary>
        /// Сумма пени предъявленная в суд
        /// </summary>
        public double? SumPenySendCourt { get; set; }
        /// <summary>
        ///Сумма госпошлины( указанная в иске)
        /// </summary>
        public double? SumStateDuty { get; set; }
        /// <summary>
        ///период задолженности начальный
        /// </summary>
        public DateTime? PeriodDebtBegin { get; set; }
        /// <summary>
        ///период задолженности конечный
        /// </summary>
        public DateTime? PeriodDebtEnd { get; set; }
        /// <summary>
        ///Реквизиты ГП - сумма
        /// </summary>
        public double? GPDetailsAmount { get; set; }
        /// <summary>
        ///Реквизиты ГП - дата платежного поручения
        /// </summary>
        public DateTime? GPDetailsDatePayment { get; set; }
        /// <summary>
        ///Реквизиты ГП - номер платежного поручения
        /// </summary>
        public string GPDetailsPaymentOrderNuumber { get; set; }
        /// <summary>
        ///Дата решения
        /// </summary>
        public DateTime? DateDecision { get; set; }
        /// <summary>
        ///Дата вступления решения в з.с.
        /// </summary>
        public DateTime? DateEntryDecision { get; set; }
        /// <summary>
        ///Дата запроса ИЛ
        /// </summary>
        public DateTime? RequestDateIl { get; set; }
        /// <summary>
        ///Дата выдачи ИЛ
        /// </summary>
        public DateTime? DateIssueIL { get; set; }
        /// <summary>
        ///Номер ИЛ
        /// </summary>
        public string NumberIl { get; set; }
        /// <summary>
        ///Реквизиты ГП - дата платежного поручения
        /// </summary>
        public string Comment { get; set; }

        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
