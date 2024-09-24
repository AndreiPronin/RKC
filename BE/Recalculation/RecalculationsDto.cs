using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Recalculation
{
    public class RecalculationsDto
    {
        public List<Recalculation> Recalculations { get; set; }
    }

    public class Recalculation
    {
        public DateTime recalculationBeginningPeriod { get; set; }
        public DateTime recalculationEndingPeriod { get; set; }
        public decimal Area { get; set; } = 0;
        public int ResidentsNumber { get; set; } = 0;
        public List<Price> prices { get; set; }
    }
    public class Price
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double Accured {  get; set; }
        public double Recalculatied { get; set; }
        public double OverallAccrued { get; set; }
        public double Tariff { get; set; }
        public double Normative { get; set; }
    }
}
