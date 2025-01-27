using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model.PaymentModel
{
    /// <summary>
    /// Платежный документ
    /// </summary>
    [Table(name: "Payments",Schema = "dbo")]
    public class PaymentEntity
    {

        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime DtCreate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата обновления записи
        /// </summary>
        public DateTime DtUpdate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата и время поступления платежа
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Идентификатор платежного инструмента
        /// </summary>
        public string PaymentInstrument { get; set; }

        /// <summary>
        /// Номер банковского документа
        /// </summary>
        public string BankDocumentNumber { get; set; }

        /// <summary>
        /// Уникальный номер операции
        /// </summary>
        public string TransactionNumberUnique { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string Lic { get; set; }

        /// <summary>
        /// Единый номер лицевого счета в ГИС ЖКХ
        /// </summary>
        public string ELic { get; set; }

        /// <summary>
        /// Идентификатор жилищно-коммунальных услуг
        /// </summary>
        public string Igku { get; set; }

        /// <summary>
        /// ФИО плательщика
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Период оплаты от клиента
        /// </summary>
        public DateTime? PaymentPeriod { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public decimal TransactionAmount { get; set; } = 0;

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal TransferAmount { get; set; } = 0;

        /// <summary>
        /// Сумма комиссии банку
        /// </summary>
        public decimal BankCommissionAmount { get; set; } = 0;

        /// <summary>
        /// Дата платежного дня
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? PaymentDateDay { get; set; }

        /// <summary>
        /// Заметка
        /// </summary>
        public string Comment { get; set; }


        


        /// <summary>
        /// Тип платежного документа
        /// 0 - Ошибка
        /// 1 - Платеж
        /// 2 - Счетчик
        /// 3 - Платеж ОДПУ
        /// </summary>
        public enum ChoicePaymentType
        {
            /// <summary>
            /// Ошибка
            /// </summary>
            Error = 0,

            /// <summary>
            /// Платеж
            /// </summary>
            Payment = 1,

            /// <summary>
            /// Счетчик
            /// </summary>
            Counter = 2,

            /// <summary>
            /// Платеж ОДПУ
            /// </summary>
            PaymentOdpu = 3,

        }

    }
}
