using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model.PaymentModel
{
    /// <summary>
    /// Финансовый период
    /// </summary>
    public class PeriodEntity 
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Дата периода
        /// </summary>
        public DateTime DtPeriod { get; set; }

        /// <summary>
        /// Дата открытия периода
        /// </summary>
        public DateTime DtCreate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата закрытие периода
        /// </summary>
        public DateTime? DtClose { get; set; } = null;

        /// <summary>
        /// Статус (0 - Закрыт, 1 - Открыт, 2 - В процессе закрытия)
        /// </summary>
        public ChoiceStatus Status { get; set; } = ChoiceStatus.Close;

        /// <summary>
        /// Период закрыт
        /// </summary>
        [NotMapped]
        public bool IsClose { get { return Status == ChoiceStatus.Close; }  }


        /// <summary>
        /// Идентификатор организации или ИП
        /// </summary>
        public int OrgId { get; set; }

        /// <summary>
        /// Организация или ИП
        /// </summary>
        public virtual OrgEntity Org { get; set; }


        /// <summary>
        /// Статус
        /// 0 - Закрыт
        /// 1 - Открыт
        /// 2 - В процессе закрытия
        /// </summary>
        public enum ChoiceStatus
        {
            /// <summary>
            /// Закрыт
            /// </summary>
            Close = 0,

            /// <summary>
            /// Открыт
            /// </summary>
            Open = 1,

            /// <summary>
            /// В процессе закрытия
            /// </summary>
            Processed = 2,
        }
    }
}
