﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public enum Banks : int
    {
        VTB = 0,
        POCHTA = 1,
        ConnectionLost = 2,
        OutlierReading = 3
    }
    public enum TypePU 
    {
        [Description("ГВС1")]
        GVS1 = 1,
        [Description("ГВС2")]
        GVS2 = 2,
        [Description("ГВС3")]
        GVS3 = 3,
        [Description("ГВС4")]
        GVS4 = 4,
        [Description("ОТП1")]
        ITP1 = 5,
        [Description("ОТП2")]
        ITP2 = 6,
        [Description("ОТП3")]
        ITP3 = 7,
        [Description("ОТП4")]
        ITP4 = 8,

    }
    public enum TypeFile
    {
        [Description("Counters")]
        Counters = 1,
        [Description("Lic")]
        Lic = 2,
        [Description("General")]
        General = 3
    }


}