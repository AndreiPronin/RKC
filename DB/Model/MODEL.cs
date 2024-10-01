using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "MODEL",Schema ="IPU")]
    public class MODEL
    {
        [Key]
        public int ID { get; set;}
        public string MODEL_NAME { get; set;}
        public BRAND BRAND { get; set;}
    }
}
