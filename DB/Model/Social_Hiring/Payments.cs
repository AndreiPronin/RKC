using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Social_Hiring
{
    [Table(name: "Payments", Schema = "dbo")]
    public class Payments
    {
        public int PaymentId { get; set; }
        public DateTime? PERIOD { get; set; }
        public string DATA { get; set; }
        public string FULL_LIC { get; set; }
        public int LIC { get; set; }
        public decimal KVPL { get; set; }
        public decimal PENY { get; set; }
        public string ERR { get; set; }
        public string PO { get; set; }
        public int SB { get; set; }
        public string DATA_VV { get; set; }
        public DateTime? DATA_PAY { get; set; }
        public DateTime? DATA_PAY_VV { get; set; }
    }


}
