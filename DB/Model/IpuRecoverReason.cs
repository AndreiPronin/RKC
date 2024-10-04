using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model
{
    [Table(name: "IpuRecoverReason",Schema = "dic")]
    public class IpuRecoverReason
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GisId { get; set; }
        public string Parameter { get; set; }
        public bool IsActual { get; set; }
        public int? NSICode { get; set; }
        public int? DictionaryNumber { get; set; }
    }
}
