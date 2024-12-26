using BE.Extenstons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BE.Recalculation
{
    public class ApplyCalculation
    {
        public string FullLic { get; set; }
        public string AdditionalFullLic { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Period { get; set; }
        public int RecalculationReason { get; set; }
        public List<Recalculation> recalculations { get; set; }
        public string RecalculationOwner { get; set; }
        public string Comment { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }
}
