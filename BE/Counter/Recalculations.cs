using System;

namespace BE.Counter
{
    public class Recalculations
    {
        public int Id { get; set; }
        public int Cadr { get; set; }
        public int ShortLic { get; set; }
        public string FullLic { get; set; }
        public int Idn { get; set; }
        public int RecalculationReasonId { get; set; }
        public int ServiceId { get; set; }
        public decimal RecalculationValue { get; set; }
        public DateTime RecalculationBeginPeriod { get; set; }
        public DateTime RecalculationEndPeriod { get; set; }
        public string RecalculationOwner { get; set; }
        public string Comment { get; set; }
        public DateTime Period { get; set; }
        public RecalculationReason RecalculationReason { get; set; }
        public Service Service { get; set; }
    }
    public class RecalculationReason
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
