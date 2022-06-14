using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class DBPersData:DbContext
    {
        //public DbSet<PersDatas> PersData { get; set; }
        public DBPersData() : base("Tplus_SRV")
        {
        }
        
    }
}
