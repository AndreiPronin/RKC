
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtInstallmentPlan
    {
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// Дата  принятия заявления на  реструктуризацию (рассрочку)
        /// </summary>
        public DateTime? DateAcceptanceApplicationRestructuring { get; set; }
        /// <summary>
        /// Сумма Реструктуризации (рассрочки)
        /// </summary>
        public double? AmountRestructuring { get; set; }
        /// <summary>
        /// Начальный месяц реструктуризации
        /// </summary>
        public DateTime? StartingMonthRestructuring { get; set; }
        /// <summary>
        /// Конечный месяц реструктуризации
        /// </summary>
        public DateTime? FinalMonthRestructuring { get; set; }
        /// <summary>
        /// Сумма ежемесячного платежа по реструктуризации
        /// </summary>
        public double? AmountMonthlyRestructuringPayment { get; set; }
        /// <summary>
        /// Дата оплаты по реструктуризации
        /// </summary>
        public DateTime? RestructuringPaymentDate { get; set; }
        /// <summary>
        /// Дата начала платежей
        /// </summary>
        public DateTime? DateStartPayment { get; set; }
        /// <summary>
        /// Дата окончания платежей
        /// </summary>
        public DateTime? DateEndPayment { get; set; }
        /// <summary>
        /// Сумма оплаты по реструктуризации
        /// </summary>
        public double? AmountPaymentRestructuring { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
