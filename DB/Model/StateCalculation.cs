using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "StateCalculation",Schema = "dbo")]
    public class StateCalculation
    {
        [Key]
        public DateTime? Period { get; set; }
        public decimal? Dk { get; set; }
        public string F4ENUMELS { get; set; }
        public decimal? PENY_DK { get; set; }
        public decimal? DkPeny_dk { get; set; }
        public decimal? Sp { get; set; }
        public decimal? Peny { get; set; }
        public decimal? SpPeny { get; set; }
        public decimal? Sn { get; set; }
        public decimal? Peny_SN { get; set; }
        public decimal? SnPeny_SN { get; set; }
        //public decimal? Sr { get; set; }
        //public decimal? Peny_SR { get; set; }
        //public decimal? SrPeny_SR { get; set; }
        public decimal? Tdk { get; set; }
        public decimal? Peny_tdk { get; set; }
        public decimal? TdkPeny_tdk { get; set; }
    }
}
