using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "FLAT",Schema = "dbo")]
    public class FLAT
    {
        [Key]
        public int id { get; set; }
        public string system_ { get; set; }
        public string object_id { get; set; }
        public string object_t { get; set; }
        public int? parent_id { get; set; }
        public string fias { get; set; }
        public string street { get; set; }
        public string type_street { get; set; }
        public string home { get; set; }
        public string building { get; set; }
        public string apartment { get; set; }
        public string fio { get; set; }
        public double? square_all { get; set; }
        public string s_notp { get; set; }
        public string giloe { get; set; }
        public string gvs { get; set; }
        public string ipu_gvs { get; set; }
        public string ipu_otp { get; set; }
        public string cadastral_number { get; set; }
        public DateTime? date_edit { get; set; }
    }


}
