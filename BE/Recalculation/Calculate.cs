using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Recalculation
{
    public class Calculate
    {
        public string FullLic { get; set; }
        public DateTime Period { get; set; }
        public DateTime RecalculationBeginningPeriod { get; set; }
        public DateTime RecalculationPeriod {  get; set; }
        public DateTime RecalculationEndingPeriod { get; set; }
        public int RecalculationReason { get; set; }
        public int SummingType { get; set; }
        public int ResidentsCount { get; set; }
        public double Volume { get; set; }
        public int CounterType { get; set; }
        public string ConcreteCounter { get; set; }
    }
}
