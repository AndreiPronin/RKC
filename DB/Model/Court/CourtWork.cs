using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtWork
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        public double? SumDebtNowDate { get; set; }
        public double? SumDebtSendCourt { get; set; }
        public double? SumOdSendCourt { get; set; }
        public double? SumPenySendCourt { get; set; }
        public double? SumGP { get; set; }
        public string RequisitesGP { get; set; }
        public DateTime? PeriodDebtBegin { get; set; }
        public DateTime? PeriodDebtEnd { get; set; }
        public string FioSendCourt { get; set; }
        public string NameCourt { get; set; }
        public DateTime? DateReceptionCourt { get; set; }
        public DateTime? DateReturnCourtSP { get; set; }
        public string NumberSP { get; set; }
        public DateTime? DateSP { get; set; }
        public double? SumPayAll { get; set; }
        public double? SumPayOD { get; set; }
        public DateTime? DatePayOD{ get; set; }
        public double? SumPayPeny { get; set; }
        public DateTime? DatePayPeny { get; set; }
        public double? SumPayGP { get; set; }
        public DateTime? DatePayGP { get; set; }
        public string Comment { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
