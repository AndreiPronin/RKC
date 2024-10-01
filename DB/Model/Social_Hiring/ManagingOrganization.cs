using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Social_Hiring
{
    [Table(name: "ManagingOrganization", Schema = "dbo")]
    public class ManagingOrganization
    {
        [Key]
        public int COD_UO { get; set; }
        public string NAIM { get; set; }
    }
}
