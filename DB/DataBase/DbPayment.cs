using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class DbPayment:DbContext
    {
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Counters> Counter { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbPayment() : base("rbr_register_payment_bank")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Counters>()
                .HasRequired<Payment>(c => c.Payment)
                .WithMany(p => p.Counter)
                .HasForeignKey(c => c.payment_id);
            modelBuilder.Entity<Payment>()
               .HasRequired<Organization>(c => c.Organization)
               .WithMany(p => p.Payment)
               .HasForeignKey(c => c.bank_id);
        }
    }
}
