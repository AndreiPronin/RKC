using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public class IpuGisReading
    {
        public int IdPu { get; set; }
        public string FulLic { get; set; }
        public string Type { get; set; }
        public string FactoryNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string GisId { get; set; }
        public string IdGku { get; set; }
        public string UniqueApartmentNumber { get; set; }
        public string Fias { get; set; }
        public DateTime? DateCheck { get; set; }
        public DateTime? DateCheckNext { get; set; }
        public string Address { get; set; }
        public string ServiceType { get; set; }
        public decimal? FinalReadings { get; set; }
        public string Dimension { get; set; }
        public DateTime? InstallationDate { get; set; }

        public IpuGisReading()
        {
        }
    }
    public class IpuGisReadingActive : IpuGisReading
    {
        public bool HasNewReadings { get; set; }
    }
}
