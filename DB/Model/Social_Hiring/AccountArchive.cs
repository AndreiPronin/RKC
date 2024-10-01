using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Social_Hiring
{
    [Table(name: "AccountArchive",Schema = "dbo")]
    public class AccountArchive
    {
        [Key]
        public int AccountId { get; set; }
        public DateTime? ArchivingDate { get; set; }
        public DateTime PERIOD { get; set; }
        public int CADR { get; set; }
        public string KW { get; set; }
        public int LIC { get; set; }
        public string FULL_LIC { get; set; }
        public decimal DK { get; set; }
        public decimal TDK { get; set; }
        public decimal SP { get; set; }
        public int KL { get; set; }
        public decimal SOBS { get; set; }
        public string N8 { get; set; }
        public string N9 { get; set; }
        public decimal SN8 { get; set; }
        public decimal SN9 { get; set; }
        public decimal SR8 { get; set; }
        public decimal SR9 { get; set; }
        public string ACT { get; set; }
        public string ZAK { get; set; }
        public decimal PENY { get; set; }
        public string PRIM { get; set; }
        public DateTime DATENOPENY { get; set; }
        public decimal PENY_DK { get; set; }
        public decimal PENY_SN { get; set; }
        public decimal PENY_SR { get; set; }
        public decimal PENY_TDK { get; set; }
        public string FAMIL { get; set; }
        public string IMYA { get; set; }
        public string OTCH { get; set; }
    }


}
