using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "FlatType",Schema = "dbo")]
    public class FlatTypeDto
    {
        [Key]
        public string FlatTypeId { get; set; }
        public string FlatType { get; set; }
    }
}
