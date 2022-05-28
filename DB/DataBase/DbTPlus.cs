using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class DbTPlus: DbContext
    {
        public DbSet<IPU_COUNTERS> IPU_COUNTERS { get; set; }
        public DbSet<IPU> IPU { get; set; }
        public DbSet<IPU_counters_PE> IPU_counters_PE { get; set; }
        public DbSet<IPU_LIC> iPU_LICs { get; set; }
        public DbTPlus() : base("T+")
        {
        }
    }
}
