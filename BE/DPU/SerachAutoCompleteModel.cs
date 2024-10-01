using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DPU
{
    public class SerachAutoCompleteModel
    {
        public int Id { get;set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Cadr { get; set; }
        public string Flat { get; set; }
        public string FullName { get; set; }
        public string NewFullLic { get; set; }
        public DateTime? Period { get; set; }
    }
}
