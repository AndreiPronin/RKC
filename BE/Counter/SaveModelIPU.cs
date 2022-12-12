using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public class SaveModelIPU
    {
        public int IdPU { get; set; }
        public string FULL_LIC { get; set; }
        public string NumberPU { get; set; }
        public string MODEL_PU { get; set; }
        public DateTime? DATE_CHECK { get; set; }
        public DateTime? DATE_CHECK_NEXT { get; set; }
        public DateTime? INSTALLATIONDATE { get; set; }
        public string SEALNUMBER { get; set; }
        public string TYPEOFSEAL { get; set; }
        public string SEALNUMBER2 { get; set; }
        public string TYPEOFSEAL2 { get; set; }
        public string TypePU { get; set; }
        public string DESCRIPTION  { get; set; }
        public bool OVERWRITE_SEAL { get; set; }
        public DateTime? CHECKPOINT_DATE { get; set; }   
        public double? CHECKPOINT_READINGS { get; set; }
        public DateTime? OPERATOR_CLOSE_DATE { get; set; }
        public double? OPERATOR_CLOSE_READINGS { get; set; }
        public decimal? FKUB2XVS { get; set; }
        public decimal? FKUB2XV_2 { get; set; }
        public decimal? FKUB2XV_3 { get; set; }
        public decimal? FKUB2XV_4 { get; set; }
        public decimal? FKUB2OT_1 { get; set; }
        public decimal? FKUB2OT_2 { get; set; }
        public decimal? FKUB2OT_3 { get; set; }
        public decimal? FKUB2OT_4 { get; set; }
        public decimal? FKUB1XVS { get; set; }
        public decimal? FKUB1XV_2 { get; set; }
        public decimal? FKUB1XV_3 { get; set; }
        public decimal? FKUB1XV_4 { get; set; }
        public decimal? FKUB1OT_1 { get; set; }
        public decimal? FKUB1OT_2 { get; set; }
        public decimal? FKUB1OT_3 { get; set; }
        public decimal? FKUB1OT_4 { get; set; }
    }
}
