using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "DPUHelpCalculationInstallation", Schema = "dbo")]
    public class DPUHelpCalculationInstallation
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Period { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Cadr { get; set; }
        public string Flat { get; set; }
        public string Lic { get; set; }
        public int? Kl { get; set; }
        public double? Sobs { get; set; }
        public string FullName { get; set; }
        public string FillLic { get; set; }
        public string NewFullLic { get; set; }
        public double? TotalAreaOfResidentialPremises { get; set; }
        public double? ShareInCommonOwnership { get; set; }
        public double? OneTimePayment { get; set; }
        public double? CostDpuResidentialPremises { get; set; }
        public string Note { get; set; }
        public double? TotalCostOdpu { get; set; }
        public double? TotalCostOdpuResidentialPremises { get; set; }
        public double? TotalCostOdpuNonResidentialPremises { get; set; }
        public double? TotalAreaMKD { get; set; }
        public double? TotalAreaMKDResidentialPremises { get; set; }
        public double? TotalAreaMKDNonResidentialPremises { get; set; }
        public double? SaldoBeginningPeriod { get; set; }
        public double? TotalAccrued { get; set; }
        public double? AccruedMainPayment { get; set; }
        public double? PercentageRate { get; set; }
        public double? PercentageRateOneMonth { get; set; }
        public double? AccruedPercentage { get; set; }
        public double? Paid { get; set; }
        public DateTime? DatePayment { get; set; }
        public double? PercentagePayment { get; set; }
        public double? PaymentMainDebt { get; set; }
        public double? ToPay { get; set; }
        public double? SaldoEndPeriodDebt { get; set; }
        public double? SaldoEndPeriodPercentage { get; set; }
    }


}
