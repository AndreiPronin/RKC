using DB.Extention;
using DB.Model;
using System.Data.Entity;


namespace DB.DataBase
{
    public class DbLIC : DbContext
    {
        public DbSet<ALL_LICS_ARCHIVE> ALL_LICS_ARCHIVE { get; set; }
        public DbSet<ALL_LICS> ALL_LICS { get; set; }
        public DbSet<KVIT> KVIT { get; set; }
        public DbSet<FlatTypeDto> FlatTypes { get; set; }
        public DbLIC() : base("DBF_SQL")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DbLicConfiguration());
            modelBuilder.Entity<KVIT>()
            .Property(e => e.lic).HasColumnType("VARCHAR").HasMaxLength(6);
        }
    }
}
