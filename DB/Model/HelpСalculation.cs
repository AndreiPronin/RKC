using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{

    [Table(name: "HelpСalculations", Schema = "dbo")]
    public class HelpСalculations
    {
        public string LIC { get; set; }
        public string FIO { get; set; }
        public string UL { get; set; }
        public string DOM { get; set; }
        public string KW { get; set; }
        public decimal? NumberPerson { get; set; }
        public decimal? Square { get; set; }
        [Key]
        public DateTime? Period { get; set; }
        public decimal? DK { get; set; }
        public decimal? Peny_dk { get; set; }
        public decimal? TdkPeny_tdk { get; set; }
        public decimal? SN { get; set; }
        public decimal? PenySNpenySR { get; set; }
        public decimal? Sp { get; set; }
        public decimal? Peny { get; set; }
        public decimal? Tdk { get; set; }
        public decimal? Peny_tdk { get; set; }
        public decimal? HeatingСalculation { get; set; }
        public decimal? HeatingRecalculation { get; set; }
        public decimal? GvsHeatingСalculation { get; set; }
        public decimal? GvsHeatingRecalculation { get; set; }
        public decimal? HvHeatingСalculation { get; set; }
        public decimal? HvHeatingRecalculation { get; set; }
        public decimal? SN15 { get; set; }
    }
    /// <summary>
    /// Old Version
    /// </summary>
    [Table(name: "HelpСalculation", Schema = "dbo")]
    public class HelpСalculation
    {
        public string FULL_LIC { get; set; }
        [Key]
        public DateTime? Period { get; set; }
        public decimal? DK { get; set; }
        public decimal? PENY_DK { get; set; }
        public decimal? SumSn { get; set; }
        public decimal? SumPenySnSr { get; set; }
        public decimal? SP { get; set; }
        public decimal? PENY { get; set; }
        public decimal? TDK { get; set; }
        public decimal? PenyTdk { get; set; }
        public decimal? SumTdkPeny_tdk { get; set; }
        public decimal? HeatingСalculation { get; set; }
        public decimal? HeatingRecalculation { get; set; }
        public decimal? GvsHeatingСalculation { get; set; }
        public decimal? GvsHeatingRecalculation { get; set; }
        public decimal? HvHeatingСalculation { get; set; }
        public decimal? HvHeatingRecalculation { get; set; }
        public decimal? SN15 { get; set; }
        public decimal? FKUB1XVS { get; set; }
        public decimal? FKUB2XVS { get; set; }
        public decimal? FKUB1XV_2 { get; set; }
        public decimal? FKUB2XV_2 { get; set; }
        public decimal? FKUB1XV_3 { get; set; }
        public decimal? FKUB2XV_3 { get; set; }
        public decimal? FKUB1XV_4 { get; set; }
        public decimal? FKUB2XV_4 { get; set; }
        public decimal? FKUB1OT_1 { get; set; }
        public decimal? FKUB2OT_1 { get; set; }
        public decimal? FKUB1OT_2 { get; set; }
        public decimal? FKUB2OT_2 { get; set; }
        public decimal? FKUB1OT_3 { get; set; }
        public decimal? FKUB2OT_3 { get; set; }
    }


}
