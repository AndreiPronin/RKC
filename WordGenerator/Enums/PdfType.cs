using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGenerator.Enums
{
    public enum PdfType
    {
        [Description("Personal")]
        Personal = 0,
        [Description("Dpu")]
        Dpu = 1,
        [Description("NewDpu")]
        NewDpu = 2,
        [Description("PersonalCabinter")]
        PersonalCabinter = 3
    }
}
