using DB.Model;
using DB.Model.PaymentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase.Configuration
{
    class PaymentV2CounterEntityConfiguration : EntityTypeConfiguration<CounterEntity>
    {
        public PaymentV2CounterEntityConfiguration()
        {
            this.Property(e => e.Value).HasPrecision(12, 5);
        }
    }
}
