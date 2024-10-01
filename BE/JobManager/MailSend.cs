using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.JobManager
{
    public class ReceiptSend
    {
        public string FullLic {get;set;}
        public string Comment { get; set; }
        public string Email { get; set; }
        public bool IsSend { get; set; }
        public DateTime DateTime { get; set; }
    }
}
