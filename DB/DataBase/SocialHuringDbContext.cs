using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class SocialHuringDbContext : DbContext
    {
        //public DbSet<>
        public SocialHuringDbContext()
           : base("SocialHuringConnection")
        {
        }
    }
}
