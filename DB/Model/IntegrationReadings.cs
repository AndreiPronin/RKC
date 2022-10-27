using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "IntegrationReadings", Schema = "dbo")]
    public class IntegrationReadings
    {
        [Key]
        public int Id { get; set; }
        public string Lic { get; set; }
        public string TypePu { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IsError { get; set; }
        public string Description { get; set; }
        public string InitialReadings { get; set; }
        public string EndReadings { get; set; }
        public string NowReadings { get; set; }
        public int? IdCounterReadings { get; set; }
    }

}
