using DB.Model.PaymentModelArchive;
using System.Data.Entity.ModelConfiguration;

namespace DB.DataBase.Configuration
{
    class PaymentV2ArhivePaymentEntityConfiguration : EntityTypeConfiguration<PaymentEntity>
    {
        public PaymentV2ArhivePaymentEntityConfiguration()
        {
            this.Property(e => e.TransactionAmount).HasPrecision(12, 5);
            this.Property(e => e.TransferAmount).HasPrecision(12, 5);
            this.Property(e => e.BankCommissionAmount).HasPrecision(12, 5);
        }
    }
}
