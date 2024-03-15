using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public class ModelAddPU
    {
        public decimal? InitialReadings { get; set; }
        public decimal? EndReadings { get; set; }
        public string FULL_LIC { get; set; }
        public int ID_PU { get; set; }
        public string FACTORY_NUMBER_PU { get; set; }
        public string GIS_ID_PU { get; set; }
        public TypePU? TYPE_PU { get; set; }
        public string BRAND_PU { get; set; }
        public string MODEL_PU { get; set; }
        public string SEAL_NUMBER { get; set; }
        public string DESCRIPTION { get; set; }
        public bool? CLOSE_ { get; set; }
        public DateTime? DATE_CHECK { get; set; }
        public DateTime? DATE_CHECK_NEXT { get; set; }
        public DateTime? DATE_INSTALL { get; set; }
        public DateTime? DATE_CLOSE { get; set; }
        public string DESCRIPTION_CLOSE { get; set; }
        public decimal? CNTR_METER_CLOSE { get; set; }
        public int? ID_USER { get; set; }
        public DateTime? TIMESTAMP { get; set; }
        public DateTime? INSTALLATIONDATE { get; set; }
        public string SEALNUMBER { get; set; }
        public string TYPEOFSEAL { get; set; }
        public string SEALNUMBER2 { get; set; }
        public string TYPEOFSEAL2 { get; set; }
        public int? InterVerificationInterval { get; set; }
        public DIMENSION DIMENSION { get; set; }
    }
}
