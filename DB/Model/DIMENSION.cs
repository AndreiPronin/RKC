using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DB.Model
{
    [Table( name: "DIMENSION", Schema = "IPU")]
    public class DIMENSION
    {
        [Key]
        public int ID { get; set; }
        public string DIMENSION_NAME { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}
