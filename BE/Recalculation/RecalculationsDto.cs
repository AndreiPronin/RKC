﻿using BE.Extenstons;
using Newtonsoft.Json;
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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime recalculationBeginningPeriod { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime recalculationEndingPeriod { get; set; }
        public decimal Area { get; set; } = 0;
        public int ResidentsNumber { get; set; } = 0;
        public List<Price> prices { get; set; }
    }
    public class Price
    {
        public int id { get; set; }
        public string name { get; set; }
        [JsonConverter(typeof(CustomDoubleConverter))]
        public double price { get; set; }
        public double Accured {  get; set; }
        public double Recalculatied { get; set; }
        public double OverallAccrued { get; set; }
        public double Tariff { get; set; }
        public double Normative { get; set; }
    }
}
