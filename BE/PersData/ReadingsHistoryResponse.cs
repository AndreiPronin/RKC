using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.PersData
{
    public class ReadingsHistoryResponse
    {
        public DateTime? PaymentDateDay { get; set; }
        public string OrganizationName { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
