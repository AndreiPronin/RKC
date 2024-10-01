using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtStateDuty
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// Дата направления на возврат г/п в ФНС
        /// </summary>
        public DateTime? DateSendOnReturnFNS { get; set; }
        /// <summary>
        /// Дата возврата заявления ФНС
        /// </summary>
        public DateTime? DateReturnFNS { get; set; }
        /// <summary>
        /// Причина возврата заявления ФНС
        /// </summary>
        public string ReasonReturn { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
