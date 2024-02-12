using DB.Model;
using DB.ViewModel;
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
        public DbSet<DIMENSION> DIMENSIONs { get; set; }
        //public DbSet<FLAT> FLAT { get; set; }
        //public DbSet<MKD> MKD { get; set; }
        public DbSet<IPU> IPU { get; set; }
        public DbSet<vw_TplusIPU_GVS> vw_TplusIPU_GVS { get; set; }
        public DbSet<vw_TplusIPU_OTP> vw_TplusIPU_OTP { get; set; }
        public DbSet<IPU_LIC> iPU_LICs { get; set; }
        public DbSet<BRAND> BRAND { get; set; }
        public DbSet<MODEL> MODEL { get; set; }
        public DbSet<AddressMKD> addresses { get; set; }
        public DbSet<FlatMkd> flats { get; set; }
        public DbSet<AddressReadings> addressReadings { get; set; }
        public DbTPlus() : base("T+")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("IPU");
            //modelBuilder.Entity<FLAT>().ToTable("FLAT","dbo");
            //modelBuilder.Entity<MKD>().ToTable("MKD", "dbo");
        }
    }
}
