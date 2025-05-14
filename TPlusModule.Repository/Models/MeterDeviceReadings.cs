using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPlusModule.Repository.Models
{
    public class MeterDeviceReadings
    {
        public string Lic { get; set; }
        public DateTime Period { get; set; }
        public int TypeId { get; set; }
        public int MeasureId { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal CurrentValue { get; set; }
        public DateTime? ReadingsDate { get; set; }
        public string? FactroyNumber { get; set; }
    }
}
