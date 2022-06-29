using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table( name: "PersonalInformation", Schema = "dbo")]
    public class PersonalInformation
    {
        public decimal? cadr { get; set; }
        public string full_lic { get; set; }
        public string els { get; set; }
        public string igku { get; set; }
        [Key]
        public string fias { get; set; }
        public string famil { get; set; }
        public string imya { get; set; }
        public string otch { get; set; }
        public string ul { get; set; }
        public string dom { get; set; }
        public string kw { get; set; }
        public string adr_gis { get; set; }
        public decimal? KL { get; set; }
        public decimal? SOBS { get; set; }
        public decimal? TARIF2 { get; set; }
        public decimal? TARIF3 { get; set; }
        public decimal? TARIF5 { get; set; }
        public decimal? S_GIL { get; set; }
        public decimal? S_NEZ { get; set; }
        public decimal? S_NOTP { get; set; }
        public decimal? S_OI { get; set; }
    }
}
