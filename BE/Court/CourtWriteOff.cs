using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtWriteOff
    {
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// Документы подготовлены к списанию (да/нет)
        /// </summary>
        public string DocumentsPreparedWriteOff { get; set; }
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
        public string DateWriteOff { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
