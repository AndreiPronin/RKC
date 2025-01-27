using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.PersData
{
    public class PaymentHistoryResponse
    {
        public DateTime? PaymentDateDay {  get; set; }
        public DateTime? PaymentDate {  get; set; }
        public decimal TransactionAmount {  get; set; }
        public string OrganizationName {  get; set; }

    }
}
