using DB.Model.PaymentModelArchive;
using System.Data.Entity.ModelConfiguration;

namespace DB.DataBase.Configuration
{
    class PaymentV2ArhiveCounterEntityConfiguration : EntityTypeConfiguration<CounterEntity>
    {
        public PaymentV2ArhiveCounterEntityConfiguration()
        {
            this.Property(e => e.Value).HasPrecision(12, 5);
        }
    }
}
