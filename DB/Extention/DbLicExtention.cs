using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Extention
{
    class DbLicExtention : EntityTypeConfiguration<ALL_LICS>
    {
        public DbLicExtention()
        {
            this.Property(e => e.FKUB2XVS).HasPrecision(11, 3);
            this.Property(e => e.FKUB2XV_2).HasPrecision(11, 3);
            this.Property(e => e.FKUB2XV_3).HasPrecision(11, 3);
            this.Property(e => e.FKUB2XV_4).HasPrecision(11, 3);
            this.Property(e => e.FKUB2OT_1).HasPrecision(11, 3);
            this.Property(e => e.FKUB2OT_2).HasPrecision(11, 3);
            this.Property(e => e.FKUB2OT_3).HasPrecision(11, 3);
            this.Property(e => e.FKUB2OT_4).HasPrecision(11, 3);
        }
    }

}
