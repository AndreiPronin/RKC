﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.FunctionModel
{
    public class RecalculationsForMKDByCadr
    {
        public int Cadr { get; set; }
        public DateTime Period { get; set; }
        public string ServiceName { get; set; }
        public string RecalculationReason { get; set; }
        public decimal RecalculationValue { get; set;}
        public string Note { get; set;}
    }
}
