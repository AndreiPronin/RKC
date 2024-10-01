using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "ReceiptNotSend", Schema = "dbo")]
    public class NotSendReceipt
    {
        public int Id { get; set; }
        public string Lic { get; set; }
        public string Email { get; set; }
        public int Month { get; set; }
        public string ErrorDescription { get; set; }
        public bool IsSend { get; set; }
        public int TypeReceipt { get; set; }
        public int NumberAttempts { get; set; }
        public DateTime DateTimeSend { get; set; }
    }
}
