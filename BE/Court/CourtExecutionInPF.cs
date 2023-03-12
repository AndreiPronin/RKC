using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtExecutionInPF
    {
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// ФИО сотрудника (направившего СП в ИО)
        /// </summary>
        public string FioSendSpInIo { get; set; }
        /// <summary>
        /// Дата передачи СП в ИО
        /// </summary>
        public DateTime? DateSendSpInIo { set; get; }
        /// <summary>
        /// Исполнительный орган (ФССП, ПФ, Банк)
        /// </summary>
        public string ExecutiveAgency { get; set; }
        /// <summary>
        /// Адрес ИО
        /// </summary>
        public string AdresIo { get; set; }
        /// <summary>
        /// Дата отправки заявления+ИД в ПФ о взыскании
        /// </summary>
        public DateTime? DateSendApplicationIdInPf { set; get; }
        /// <summary>
        /// Способ отправки оригиналов заявления+ИД в ПФ
        /// </summary>
        public string WaySendOriginalApplicationIdInPf { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - всего
        /// </summary>
        public double? SumApplicationPfAll { get;set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - ОД
        /// </summary>
        public double? SumApplicationPfOd { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - пени
        /// </summary>
        public double? SumApplicationPfPeny { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - ГП
        /// </summary>
        public double? SumApplicationPfGp { get; set; }
        /// <summary>
        /// Дата получения к исполнению ПФ
        /// </summary>
        public DateTime? DateReturnPF { set; get; }
        /// <summary>
        /// Срок нахождения на исполнени (дн.)
        /// </summary>
        public int? LengthStayDay { get; set; }
        /// <summary>
        /// Дата возврата ИД из ПФ
        /// </summary>
        public DateTime? DateReturnIdPF { set; get; }
        /// <summary>
        /// Дата отзыва ИД
        /// </summary>
        public DateTime? DateReturnId { set; get; }
        /// <summary>
        /// Причина отзыва/возврата ИД из ПФ
        /// </summary>
        public string ReasonReturnIdPf { get; set; }
        /// <summary>
        /// Исх. № письма от ПФ о возврате ИД
        /// </summary>
        public string NumberMailPfReturnId { get; set; }
        /// <summary>
        /// Сумма исполнения от ПФ
        /// </summary>
        public double SumExecutionPf { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
