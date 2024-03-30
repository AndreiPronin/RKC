using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "Flat", Schema = "Address")]
    public class FlatMkd
    {
        [Key]
        public string FullLic { get; set; }
        public string System { get; set; }
        public string ObjectType { get; set; }
        public int AddressId { get; set; }
        public string UniqueApartmentNumber { get; set; }
        public string UniqueRoomNumber { get; set; }
        public string CadastrNumberFlat { get; set; }
        public decimal? CadastrSquare { get; set; }
        public string Els { get; set; }
        public string IdGku { get; set; }
        public DateTime? DateEdit { get; set; }
    }
}
