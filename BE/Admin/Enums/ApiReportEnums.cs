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
        [Description(nameof(TextReportsGetSberbankSevens))]
        TextReportsGetSberbankSevens = 0,
        [Description(nameof(TextReportsGetOldTypeSberbankSevens))]
        TextReportsGetOldTypeSberbankSevens = 1,
        [Description(nameof(TextReportsGetSberbankSevensWithEights))]
        TextReportsGetSberbankSevensWithEights = 2,
        [Description(nameof(TextReportsGetSberbankSevensCounters))]
        TextReportsGetSberbankSevensCounters = 3,
    }
}
