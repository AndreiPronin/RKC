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
        public Int64 id { get; set; }
        public string system { get; set; }
        public string objectId { get; set; }
        public string idCcb { get; set; }
        public string objectT { get; set; }
        public decimal parentId { get; set; }
        public string fias { get; set; }
        public string CadstraNumber { get; set; }
        public string street { get; set; }
        public string typeStreet { get; set; }
        public string home { get; set; }
        public string building { get; set; }
        public string apartment { get; set; }
        public string fio { get; set; }
        public double? squareAll { get; set; }
        public string sNotp { get; set; }
        public string giloe { get; set; }
        public string gvs { get; set; }
        public string ipuGvs { get; set;}
        public string ipuOtp { get; set;}
        public string object_disable { get; set; }
    }


}
