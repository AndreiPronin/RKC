using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPlusModule.Repository.Models;

namespace TPlusModule.Repository.Contexts
{
    public class RkcTplusContext : DbContext
    {
        public RkcTplusContext(DbContextOptions<RkcTplusContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_CI_AS");

            modelBuilder.Entity<AccountVolume>().HasIndex(x => new { x.AccountNumber, x.Period }).IsUnique();
            modelBuilder.Entity<MeterDeviceReadings>().HasIndex(x => new { x.AccountNumber, x.Period, x.MeterDeviceTypeId }).IsUnique();
        }
    }
}
