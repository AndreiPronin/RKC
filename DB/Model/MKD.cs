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
        public Int64 id { get; set; }
        public string system { get; set; }
        public int objectId { get; set; }
        public string idCcb { get; set; }
        public string objectType { get; set; }
        //public decimal parentId { get; set; }
        public string fias { get; set; }
        public int? postalCode { get; set; }
        public string street { get; set; }
        public string typeStreet { get; set; }
        public string home { get; set; }
        public string building { get; set; }
        public string apartment { get; set; }
        public string fio { get; set; }
        public double? squareObjectAll { get; set; }
        public double? squareMopAll { get; set; }
        public double? squareColdAll { get; set; }
        public string sNotp { get; set; }
        public string giloe { get; set; }
        public string gvs { get; set; }
        public string ipuGvs { get; set; }
        public string ipuOtp { get; set; }
        public string object_disable { get; set; }
        public string CadastralNumber { get; set; }
    }


}
