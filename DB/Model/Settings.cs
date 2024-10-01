using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "Settings", Schema = "dbo")]
    public class Settings
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
