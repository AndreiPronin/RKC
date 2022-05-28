using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    class modelUpdatePU
    {
        public string LIC { get; set; }
        public string TypePU { get; set; }
        public string NumberPU { get; set; }
        public DateTime? DataCheck { get; set; }
        public DateTime? DataNextCheck { get; set; }
    }
}
