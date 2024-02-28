using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "IPU", Schema = "IPU")]
    public class IPU
    {
        [Key]
        public int id { get; set; }
        public string ZAVOD_NUMBER_PU { get; set; }
        public string BRAND_PU { get; set; }
        public string MODEL_PU { get; set; }
        public string GIS_ID_PU { get; set; }
        public string FULL_LIC { get; set; }
        public decimal? N_LIC { get; set; }
        public string LINK1 { get; set; }
        public string TYPE_PU { get; set; }
        public decimal? FKUBSXVS { get; set; }
        public decimal? FKUBSXV_2 { get; set; }
        public decimal? FKUBSXV_3 { get; set; }
        public decimal? FKUBSXV_4 { get; set; }
        public decimal? FKUBSOT_1 { get; set; }
        public decimal? FKUBSOT_2 { get; set; }
        public decimal? FKUBSOT_3 { get; set; }
        public decimal? FKUBSOT_4 { get; set; }
        public string LINK2 { get; set; }
        public decimal? NUM_CNT_LINK1 { get; set; }
        public decimal? NUM_CNT_LINK2 { get; set; }
        public string ZAV_NUM_PU_ALEX { get; set; }
        public bool? DELETE { get; set; }
        //public IPU_COUNTERS IPU_COUNTERS { get; set; }
        public IPU_COUNTERS IPU_COUNTERS = new IPU_COUNTERS();
    }
    [Table(name: "IPU_COUNTERS", Schema = "IPU")]
    public class IPU_COUNTERS
    {
        [Key]
        [Column("ID_PU")]
        public int ID_PU { get; set; }
        //public List<IPU> IPU = new List<IPU>();
        public string FACTORY_NUMBER_PU { get; set; }
        public string TYPE_PU { get; set; }
        public string BRAND_PU { get; set; }
        public string MODEL_PU { get; set; }
       // [Key]
        public string GIS_ID_PU { get; set; }
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
        public string FULL_LIC { get; set; }
        public DateTime? CHECKPOINT_DATE { get;set; }
        public double? CHECKPOINT_READINGS { get; set; }
        public DateTime? OPERATOR_CLOSE_DATE { get; set; }
        public double? OPERATOR_CLOSE_READINGS { get; set; }
        public int? DIMENSION_ID { get;set;}
        public DateTime? LastReadingDate { get; set; }
        public Guid? MeteringDeviceVersionGUID { get; set; }
        /// <summary>
        /// Номер прибора учета в ГИС ЖКХ
        /// </summary>
        public string MeteringDeviceGISGKHNumber { get; set; }
        /// <summary>
        /// Идентификатор ПУ в ГИС ЖКХ
        /// </summary>
        public Guid? MeteringDeviceRootGUID { get; set; }
        public DIMENSION DIMENSION { get; set; }
        public ALL_LICS ALL_LICS = new ALL_LICS();
        public List<BRAND> BrandDictionary = new List<BRAND>();
    }
}
