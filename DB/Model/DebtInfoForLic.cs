using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    public class DebtInfoForLic
    {
        public string Lic { get; set; }
        public double Debt {  get; set; }
        public double Payment { get; set; }
        public double CurrentDebt { get; set; }
    }
}
