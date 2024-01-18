using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Admin.Enums
{
    public enum ApiReportEnums
    {
        [Description("6315376946_40702810748000001123_7_001_.txt")]
        GetSberbankInvoicesOldFormat = 0,
        [Description("6315376946_40702810748000001123_7_001_.txt")]
        GetSberbankInvoices = 1,
        [Description("COUNTERS_6315376946_40702810748000001123_7_001_.txt")]
        GetSberbankCounters = 2,
    }
}
