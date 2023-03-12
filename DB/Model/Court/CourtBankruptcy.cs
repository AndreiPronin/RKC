using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtBankruptcy
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// № банкротного дела
        /// </summary>
        public string BankruptcyCaseNumber { get; set; }
        /// <summary>
        /// Дата  определения о принятии  заявления о банкротстве судом
        /// </summary>
        public DateTime? DateDeterminationAcceptance { get; set; }
        /// <summary>
        /// Дата определения о завершении реализации имущества
        /// </summary>
        public DateTime? DateDeterminationCompletion { get; set; }
        /// <summary>
        /// Дата принятия заявления нами
        /// </summary>
        public DateTime? DateDeterminationApplication { get; set; }
        /// <summary>
        /// Сумма списания
        /// </summary>
        public double? SumWriteOff { get; set; }
        /// <summary>
        /// Начальный период списания
        /// </summary>
        public DateTime? DateWriteOffBegin { get; set; }
        /// <summary>
        /// Конечный период списания
        /// </summary>
        public DateTime? DateWriteOffEnd { get; set; }
        /// <summary>
        /// Статус списания 
        /// </summary>
        public string WriteOffStatus { get; set; }
        /// <summary>
        /// Дата списания
        /// </summary>
        public DateTime? DateWrite { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public virtual CourtGeneralInformation CourtGeneralInformation { get;set; }
    }
}
