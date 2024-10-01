using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.PersData
{
    public class PenyModel
    {
        public int id { get; set; }
        public string lic { get; set; }
        public DateTime period { get; set; }
        public double nach { get; set; }
        public DateTime srokOpl { get; set; }
        public DateTime srokS { get; set; }
        public DateTime srokPo { get; set; }
        public double opl { get; set; }
        public double dolg { get; set; }
        public int dnDolg { get; set; }
        public int dnNach { get; set; }
        public int dn300 { get; set; }
        public int dn130 { get; set; }
        public DateTime tekd1 { get; set; }
        public DateTime tekd2 { get; set; }
        public int tekDn { get; set; }
        public int tek300 { get; set; }
        public int tek130 { get; set; }
        public double d1_300 { get; set; }
        public double d1_130 { get; set; }
        public double peny300 { get; set; }
        public double peny130 { get; set; }
        public double itog { get; set; }
        public DateTime allLicPeriod { get; set; }
    }
}
