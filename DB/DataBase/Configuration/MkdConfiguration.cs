using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase.Configuration
{
    internal class MkdConfiguration : EntityTypeConfiguration<AddressMKD>
    {
        public MkdConfiguration() 
        {

            this.Property(e => e.NormOtp).HasPrecision(10, 6);
            this.Property(e => e.NormGvs).HasPrecision(10, 6);
            this.Property(e => e.NormHvs).HasPrecision(10, 6);
        }
    }
}
