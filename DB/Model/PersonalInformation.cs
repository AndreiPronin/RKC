using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "PersonalInformations", Schema = "dbo")]
    public class PersonalInformations
    {
        public string full_lic { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
        public string RoomType { get; set; }
        public decimal? NumberPerson { get; set; }
        public decimal? Square { get; set; }
        [Key]
        public string Fias { get; set; }
        public string els { get; set; }
        public string igku { get; set; }
        public decimal? TARIF2 { get; set; }
        public decimal? TARIF3 { get; set; }
        public decimal? TARIF5 { get; set; }
        public decimal? S_OI { get; set; }
        public decimal? S_NEZ { get; set; }
        public decimal? S_GIL { get; set; }
        public decimal? S_NOTP { get; set; }
    }
    /// <summary>
    /// Old 
    /// </summary>
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
