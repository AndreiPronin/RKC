using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    public class DPUSummaryHouses
    {
        [Key]
        public int Id { get; set; }
        public string Cadr { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public double? TotalCostODPU { get; set; }
        public double? TotalCostOdpuResidentialPremises { get; set; }
        public double? TotalCostOdpuNonResidentialPremises { get; set; }
        public double? TotalAreaMKD { get; set; }
        public double? TotalAreaMKDResidentialPremises { get; set; }
        public double? TotalAreaMKDNonResidentialPremises { get; set; }
        public DateTime? DateTransferRBR { get; set; }
        public DateTime? PeriodExhibit { get; set; }
    }


}
