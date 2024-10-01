using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Social_Hiring
{
    [Table(name: "Address", Schema = "dbo")]
    public class Address
    {
        [Key]
        public int CADR { get; set; }
        public string UL { get; set; }
        public string DOM { get; set; }
        public string ACT { get; set; }
        public int CP8 { get; set; }
        public int CP9 { get; set; }
        public string ZAK { get; set; }
        public int RAION { get; set; }
        public string POSTINDEX { get; set; }
        public int ETAZNOST { get; set; }
        public int COD_UO { get; set; }
        public decimal SOBSTEXP { get; set; }
        public int GODPOSTR { get; set; }
        public string FIAS { get; set; }
    }


}
