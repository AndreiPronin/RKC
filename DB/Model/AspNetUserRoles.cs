using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "AspNetUserRoles", Schema ="dbo")]
    public class AspNetUserRoles
    {
        public string UserId { get; set; }
        [Key]
        public string RoleId { get; set; }
    }
    [Table(name: "AspNetRoles", Schema = "dbo")]
    public class AspNetRoles
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    [Table(name: "AspNetUsers", Schema = "dbo")]
    public class AspNetUsers
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FIO { get; set; }
    }
}
