using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "AddressReadings", Schema = "Address")]
    public class AddressReadings
    {
        [Key]
        public int Id { get; set; }
        public int AddressId { get; set; }
        public DateTime Period { get; set; }
        public string DpuHeating { get; set; }
        public string DpuGvsColdWater { get; set; }
        public string IpuHeating { get; set; }
        public string IpuGvsThermalEnergy { get; set; }
        public string IpuGvsColdWater { get; set; }
        public string NormHeating { get; set; }
        public string NormGvsThermalEnergy { get; set; }
        public string NormGvsColdWater { get; set; }
        public string NonResidentialPremisesHeating { get; set; }
        public string NonResidentialPremisesThermalEnergy { get; set; }
        public string NonResidentialPremisesColdWater { get; set; }
        public string OdnHeating { get; set; }
        public string OdnGvsColdWater { get; set; }
    }
}
