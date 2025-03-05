using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MkdInformation
{
    public class HistoryValueOdpuModel
    {
        public DateTime Period { get; set; }
        public string HeatingPlantServiceType { get; set; }
        public string Type { get; set; }
        public string FactoryNumber { get; set; }
        public string CombinedOpu { get; set; }
        public decimal? VolumeGCal { get; set; }
        public decimal? VolumeCubeMeter { get; set; }
        public string CalculationMethod { get; set; }
        public string HeatingPlantId { get; set; }
    }
}
