using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table( name:"IPU_LIC",Schema = "IPU" )]
    public class IPU_LIC
    {
        [Key]
        public int ID_LIC { get; set; }
        public decimal N_LIC { get; set; }
        public string FULL_LIC { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string STREET { get; set; }
        public string HOME { get; set; }
        public string FLAT { get; set; }
        public bool? CLOSE_ { get; set; }
        //public decimal STATUS_GVS1 { get; set; }
        //public decimal STATUS_GVS2 { get; set; }
        //public decimal STATUS_GVS3 { get; set; }
        //public decimal STATUS_GVS4 { get; set; }
        //public decimal STATUS_OTP1 { get; set; }
        //public decimal STATUS_OTP2 { get; set; }
        //public decimal STATUS_OTP3 { get; set; }
        //public decimal STATUS_OTP4 { get; set; }
        //public int? ID_GVS1 { get; set; }
        //public int? ID_GVS2 { get; set; }
        //public int? ID_GVS3 { get; set; }
        //public int? ID_GVS4 { get; set; }
        //public int? ID_OTP1 { get; set; }
        //public int? ID_OTP2 { get; set; }
        //public int? ID_OTP3 { get; set; }
        //public int? ID_OTP4 { get; set; }
        //public bool? LIC_ARCHIVE { get; set; }
        //public int ID_USER { get; set; }
        public DateTime TIMESTAMP { get; set; }
        public decimal NRM5 { get; set; }
        public bool? PU_GVS_MULTILIC_MAIN { get; set; }
        public bool? PU_GVS_MULTILIC_SLAVE { get; set; }
        public string FIO { get; set; }
    }
}
