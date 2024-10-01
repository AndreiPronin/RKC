using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "BRAND", Schema = "IPU")]
    public class BRAND
    {
        [Key]
        public int ID { get; set; }
        public string BRAND_NAME { get; set; }
        public string TYPE_PU { get; set; }
        public List<MODEL> MODEL { get; set; }
    }
}
