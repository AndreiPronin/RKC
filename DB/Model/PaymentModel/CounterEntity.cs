using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model.PaymentModel
{ 
    /// <summary>
    /// Счетчик
    /// </summary>
    public class CounterEntity : AppBaseEntity
    {
        /// <summary>
        /// Наименование ИПУ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение ИПУ
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string Lic { get; set; }

        /// <summary>
        /// Дата и время поступления платежа
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Идентификатор платежного документа
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Платежный документа
        /// </summary>
        [ForeignKey(nameof(PaymentId))]
        public virtual PaymentEntity Payment { get; set; }



    }
}
