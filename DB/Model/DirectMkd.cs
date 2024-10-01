using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    public class DirectMkd
    {
        public long? id { get; set; }
        public string system { get; set; }
        public string objectType { get; set; }
        public int objectId { get; set; }
        public string idCcb { get; set; }
        public string fias { get; set; }
        public double? squareObjectAll { get; set; }
        public double? squareMopAll { get; set; }
        public double? squareColdAll { get; set; }
        public int? postalCode { get; set; }
        public string region { get; set; }
        public string type { get; set; }
        public string street { get; set; }
        public string typeStreet { get; set; }
        public string home { get; set; }
        public string building { get; set; }
        public string gvs { get; set; }
        public string ipuGvs { get; set; }
        public string ipuOtp { get; set; }
        public string object_disable { get; set; }
        public string CadastralNumber { get; set; }
    }


}
