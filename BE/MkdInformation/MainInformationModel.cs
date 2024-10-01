using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MkdInformation
{
    public class MainInformationModel
    {
        public AddressMKDBe AddressMKD { get; set; }
        public AddressReadingsBe AddressReadings { get; set; }
        public MainInformationModel() {
            AddressMKD = new AddressMKDBe();
            AddressReadings = new AddressReadingsBe();
        }
    }
}
