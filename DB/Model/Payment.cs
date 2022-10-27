using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "payment", Schema = "dbo")]
    public class Payment
    {
        [Key]
        public int id { get; set; }
        public DateTime? payment_date { get; set; }
        public string address { get; set; }
        public double? bank_commission_amount { get; set; }
        public string bank_document_number { get; set; }
        public string full_name { get; set; }
        public string lic { get; set; }
        public int? payment_instrument_id { get; set; }
        public DateTime? payment_period { get; set; }
        public double transaction_amount { get; set; }
        public string transaction_number_inique { get; set; }
        public double? transfer_amount { get; set; }
        public int? bank_id { get; set; }
        public string elic { get; set; }
        public string igku { get; set; }
        public DateTime? dt_create { get; set; }
        public DateTime? payment_date_day { get; set; }
        public int? log_upload_file_id { get; set; }
        public int? period_id { get; set; }
        public ICollection<Counters> Counter { get; set; }
        public Organization Organization { get; set; }
    }
    [Table(name: "Counter", Schema ="dbo")]
    public class Counters
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public int payment_id { get; set; }
        public string lic { get; set; }
        public DateTime? dt { get; set; }
        public Payment Payment { get; set; }
    }
    [Table(name: "organization", Schema = "dbo")]
    public class Organization
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Payment> Payment { get; set; }
    }




}
