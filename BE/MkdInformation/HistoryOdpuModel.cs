using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MkdInformation
{
    public class HistoryOdpuModel
    {
        public List<AddressReadingsBe> addressReadings { get; set; }
        public AddressMKDBe addressMKD { get; set; }
        public HistoryOdpuModel() { }
    }
}
