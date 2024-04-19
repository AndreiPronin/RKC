using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtWork
    {
        public int CourtGeneralInformationId { get; set; }
        public double? SumDebtNowDate { get; set; }
        public double? SumDebtSendCourt { get; set; }
        public double? SumOdSendCourt { get; set; }
        public double? AmountdebtTransferredToCourtTotal { get; set; }
        public double? SumPenySendCourt { get; set; }
        public double? SumGP { get; set; }
        public double? RequisitesSumGP { get; set; }
        public DateTime? RequisitesDateGP { get; set; }
        public string RequisitesNumberGP { get; set; }
        public double? AmountOverpaidGP { get; set; }
        public DateTime? PeriodDebtBegin { get; set; }
        public DateTime? PeriodDebtEnd { get; set; }
        public string FioSendCourt { get; set; }
        public string SubmitApplicationCourt { get; set; }
        public string NameCourt { get; set; }
        /// <summary>
        /// Адресс суда
        /// </summary>
        public string AddressCourt { get; set; }
        public DateTime? DateReceptionCourt { get; set; }
        public DateTime? DateReturnCourtSP { get; set; }
        /// <summary>
        /// Дата определения на возврат излишне уплаченной ГП
        /// </summary>
        public DateTime? DeterminationDateOverpaidGP { get; set; }
        /// <summary>
        /// Дата передачи реестра СП в бухгалтерию
        /// </summary>
        public DateTime? DateAccountingDepartment { get; set; }
        public string ReasonReturningApplication { get; set; }
        public string NumberSP { get; set; }
        public DateTime? DateSP { get; set; }
        public double? SumPayAll { get; set; }
        public double? SumPayOD { get; set; }
        public double? SumPayPeny { get; set; }
        public double? SumPayGP { get; set; }
        public string Comment { get; set; }
        /// <summary>
        /// Дата задания
        /// </summary>
        public DateTime? DateTask { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
