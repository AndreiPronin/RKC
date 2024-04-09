using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "Address", Schema = "Address")]
    public class AddressMKD
    {
        [Key]
        public int AddressId { get; set; }
        public short? RegionCode { get; set; }
        public int? Postindex { get; set; }
        public string RegionType { get; set; }
        public string Region { get; set; }
        public string CityType { get; set; }
        public string City { get; set; }
        public string StreetType { get; set; }
        public string Street { get; set; }
        public string HouseType { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string Fias { get; set; }
        public string FiasCadastr { get; set; }
        public short? BuildYear { get; set; }
        public byte? Floors { get; set; }
        public decimal? Soifl { get; set; }
        public decimal? Sgil { get; set; }
        public decimal? Snez { get; set; }
        public decimal? Snotp { get; set; }
        public decimal? Smop { get; set; }
        public decimal? Tarif2 { get; set; }
        public decimal? Tarif3 { get; set; }
        public decimal? Tarif5 { get; set; }
        public bool? Gvs { get; set; }
        public bool? OdpuGvs { get; set; }
        public bool? OdpuOtp { get; set; }
        public bool? Closed { get; set; }
        public string CadstrNumberMKD { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? Dateedit { get; set; }
        public bool? IpuGvs { get; set; }
        public bool? IpuOtp { get; set; }
        public DateTime? GvsBeginningPeriod { get; set; }
        public DateTime? OtpBeginningPeriod { get; set; }
        public decimal? NormOtp { get; set; }
        public decimal? NormGvs { get;set; }
        public decimal? NormHvs { get; set; }
        public string TPlusGuid { get; set; }
        public string UniqueHomeNumber { get; set; }
    }


}
