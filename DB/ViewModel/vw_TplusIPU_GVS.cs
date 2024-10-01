using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.ViewModel
{
    [Table(name: "view_TplusIPU_GVS", Schema = "dbo")]
    public class vw_TplusIPU_GVS
    {
        public decimal? CODE_HOUSE { get; set; }
        public string STREET { get; set; }
        public string HOME { get; set; }
        public string FLAT { get; set; }
        [Key]
        public string FULL_LIC { get; set; }
        public string FIO { get; set; }
        public string TYPE_PU { get; set; }
        public string FACTORY_NUMBER_PU { get; set; }
        public string DATE_CHECK { get; set; }
        public string DATE_CHECK_NEXT { get; set; }
        public string SEALNUMBER { get; set; }
        public string TYPEOFSEAL { get; set; }
        public string SEALNUMBER2 { get; set; }
        public string TYPEOFSEAL2 { get; set; }
        public string SIGN_PU { get; set; }
        public string END_READINGS { get; set; }
        public int? NOW_READINGS { get; set; }
    }
    public class view_TplusIPU_GVS
    {
        public decimal? CODE_HOUSE { get; set; }
        public string STREET { get; set; }
        public string HOME { get; set; }
        public string FLAT { get; set; }
        public string FULL_LIC { get; set; }
        public string FIO { get; set; }
        public string TYPE_PU { get; set; }
        public string INSTALLATIONDATE { get; set; }
        public string FACTORY_NUMBER_PU { get; set; }
        public string BRAND_PU { get; set; }
        public string MODEL_PU { get; set; }
        public string DATE_CHECK { get; set; }
        public string DATE_CHECK_NEXT { get; set; }
        public string SEALNUMBER { get; set; }
        public string TYPEOFSEAL { get; set; }
        public string SEALNUMBER2 { get; set; }
        public string TYPEOFSEAL2 { get; set; }
        public string SIGN_PU { get; set; }
        public string DIMENSION_NAME { get; set; }
        public string END_READINGS { get; set; }
        public int? NOW_READINGS { get; set; }
        public int? InterVerificationInterval { get; set; }
    }


}
