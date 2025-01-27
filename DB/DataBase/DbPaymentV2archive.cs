using DB.DataBase.Configuration;
using DB.Model.PaymentModelArchive;
using System.Data.Entity;

namespace DB.DataBase
{
    public class DbPaymentV2archive : DbContext
    {
        public DbSet<PaymentEntity> PaymentEntityArchive { get; set; }
        public DbSet<CounterEntity> CounterEntityArchive { get; set; }
        public DbPaymentV2archive() : base("rbr-load-register-payment-archive-v2")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PaymentV2ArhiveCounterEntityConfiguration());
            modelBuilder.Configurations.Add(new PaymentV2ArhivePaymentEntityConfiguration());
        }
    }
}
