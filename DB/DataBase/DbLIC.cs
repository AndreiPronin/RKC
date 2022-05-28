using DB.Extention;
using DB.Model;
using System.Data.Entity;


namespace DB.DataBase
{
    public class DbLIC : DbContext
    {
        public DbSet<ALL_LICS_ARCHIVE> ALL_LICS_ARCHIVE { get; set; }
        public DbSet<ALL_LICS> ALL_LICS { get; set; }
        public DbLIC() : base("DBF_SQL")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DbLicExtention());
        }
    }
}
