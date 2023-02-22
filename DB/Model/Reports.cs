using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "Reports", Schema ="dbo")]
    public class Reports
    {
        [Key]
        public int Id { get; set; }
        public string SqlQuery { get; set; }
        public string NameReport { get; set; }
    }
}
