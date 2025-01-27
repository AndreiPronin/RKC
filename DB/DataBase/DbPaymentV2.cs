using DB.DataBase.Configuration;
using DB.Model.PaymentModel;
using System.Data.Entity;

namespace DB.DataBase
{
    public class DbPaymentV2:DbContext
    {
        public DbSet<BankEntity> BankEntity { get; set; }
        public DbSet<CounterEntity> CounterEntity { get; set; }
        public DbSet<OrgEntity> OrgEntity { get; set; }
        public DbSet<PaymentEntity> PaymentEntity { get; set; }
        public DbSet<PeriodEntity> PeriodEntity { get; set; }

        public DbPaymentV2() : base("rbr-load-register-payment-v2")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PaymentV2CounterEntityConfiguration());
            modelBuilder.Configurations.Add(new PaymentV2PaymentEntityConfiguration());
        }
    }
}
