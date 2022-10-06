using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.ViewModel
{
    [Table(name: "vw_CounterTPlus", Schema ="dbo")]
    public class vw_CounterTPlus
    {
        public decimal? CodeHouse { get; set; }
        public string Streer { get; set; }
        public string Home { get; set; }
        public string Flat { get; set; }
        [Key]
        public string Lic { get; set; }
        public string Fio { get; set; }
        public string Pu { get; set; }
        
        public string PuNumber { get; set; }
        public string DataCheck { get; set; }
        public string DataNextCheck { get; set; }
        public string Seal { get; set; }
        public string TypeSeal { get; set; }
        public string Seal2 { get; set; }
        public string TypeSeal2 { get; set; }
        public string SignPu { get; set; }
        public string EndReadings { get; set; }
        public int? NowReadings { get; set; }
    }
}
