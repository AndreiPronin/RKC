using System;
using System.Collections.Generic;

namespace BE.Recalculation
{
    public class ApplyCalculation
    {
        public string FullLic { get; set; }
        public DateTime Period { get; set; }
        public int RecalculationReason { get; set; }
        public List<Recalculation> recalculations { get; set; }
        public string RecalculationOwner { get; set; }
        public string Comment { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
