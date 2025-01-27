using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DB.Model.PaymentModelArchive
{
    /// <summary>
    /// Платежный документ (архив)
    /// </summary>
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
        /// Дата и время поступления платежа
        /// </summary>
        public  DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Идентификатор платежного инструмента
        /// </summary>
        public  string PaymentInstrument { get; set; }

        /// <summary>
        /// Номер банковского документа
        /// </summary>
        public  string BankDocumentNumber { get; set; }

        /// <summary>
        /// Уникальный номер операции
        /// </summary>
        public  string TransactionNumberUnique { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public  string Lic { get; set; }

        /// <summary>
        /// Единый номер лицевого счета в ГИС ЖКХ
        /// </summary>
        public  string ELic { get; set; }

        /// <summary>
        /// Идентификатор жилищно-коммунальных услуг
        /// </summary>
        public  string Igku { get; set; }

        /// <summary>
        /// ФИО плательщика
        /// </summary>
        public  string FullName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public  string Address { get; set; }

        /// <summary>
        /// Период оплаты от клиента
        /// </summary>
        public  DateTime? PaymentPeriod { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>

        public  decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>

        public  decimal? TransferAmount { get; set; }

        /// <summary>
        /// Сумма комиссии банку
        /// </summary>

        public  decimal? BankCommissionAmount { get; set; }

        /// <summary>
        /// Дата платежного дня
        /// </summary>
        [Column(TypeName = "date")]
        public  DateTime? PaymentDateDay { get; set; }

        /// <summary>
        /// Наименование банка или платежной организации
        /// </summary>
        public  string RegisterBankName { get; set; }

        /// <summary>
        /// Внешний идентификатор организации
        /// </summary>
        public  string OrgOrigamiGuid { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public  string OrgName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>

        public string OrgOgrn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>

        public string OrgKpp { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string OrgInn { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>

        public string OrgCorrespondentAccount { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        public  string RequisiteName { get; set; }

        /// <summary>
        /// Расчётный счёт
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public  string RequisiteCheckingAccount { get; set; }

        /// <summary>
        /// БИК банка
        /// </summary>
        [Column(TypeName = "varchar(9)")]
        public  string RequisiteBik { get; set; }


        /// <summary>
        /// Финансовый период - год
        /// </summary>
        public  int PeriodYear { get; set; }


        /// <summary>
        /// Финансовый период - месяц
        /// </summary>
        public  int PeriodMonth { get; set; }

        /// <summary>
        /// Тип платежного документа (1 - Платеж, 2 - Счетчик, 0 - Ошибка)
        /// </summary>
        public  string PaymentType { get; set; }

        /// <summary>
        /// Показания ИПУ
        /// </summary>
        public virtual List<CounterEntity> Counters { get; set; }
    }
}
