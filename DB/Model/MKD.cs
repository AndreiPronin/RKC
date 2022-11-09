using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "MKD", Schema = "dbo")]
    public class MKD
    {
        [Key]
        public int id { get; set; }
        public string system { get; set; }
        public string object_type { get; set; }
        public int? object_id { get; set; }
        public string fias { get; set; }
        public double? square_object_all { get; set; }
        public double? square_mop_all { get; set; }
        public double? square_cold_all { get; set; }
        public int? postalCode { get; set; }
        public int? region { get; set; }
        public string type_ { get; set; }
        public string street { get; set; }
        public string type_street { get; set; }
        public string home { get; set; }
        public string building { get; set; }
        public string gvs { get; set; }
        public string ipu_gvs { get; set; }
        public string ipu_otp { get; set; }
        public DateTime? date_edit { get; set; }
    }


}
