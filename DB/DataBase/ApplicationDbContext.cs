using DB.Extention;
using DB.Model;
using DB.Model.Court;
using DB.Model.Court.DictiomaryModel;
using DB.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PersonalInformations> PersonalInformation { get; set; }
        public DbSet<StateCalculation> StateCalculation { get; set; }
        public DbSet<LogsPersData> LogsPersData { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Flags> Flags { get; set; }
        public DbSet<PersData> PersData { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<PersDataDocument> PersDataDocument { get; set; }
        public DbSet<HelpСalculations> HelpСalculation { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<IntegrationReadings> IntegrationReadings { get; set; }
        public DbSet<vw_CounterTPlus> vw_CounterTPlus { get; set; }
        //public DbSet<DPUHelpCalculationInstallationView> dPUHelpCalculationInstallations { get; set; }
        //public DbSet<DPUSummaryHousesView> dPUSummaryHouses { get; set; }
        public DbSet<CourtBankruptcy> CourtBankruptcy { get; set; }
        public DbSet<CourtWork> CourtWork { get; set; }
        public DbSet<CourtDocumentScans> CourtCourtDocumentScans { get; set; }
        public DbSet<CourtExecutionFSSP> CourtExecutionFSSP { get; set; }
        public DbSet<CourtExecutionInPF> CourtExecutionInPF { get; set; }
        public DbSet<CourtGeneralInformation> CourtGeneralInformation { get; set; }
        public DbSet<CourtInstallmentPlan> CourtInstallmentPlan { get; set; }
        public DbSet<CourtLitigationWork> CourtLitigationWork { get; set; }
        public DbSet<CourtStateDuty> CourtStateDuty { get; set; }
        public DbSet<CourtWriteOff> CourtWriteOff { get; set; }
        public DbSet<CourtNameDictionary> CourtNameDictionaries { get; set; }
        public DbSet<CourtValueDictionary> CourtValueDictionary { get; set; }
        public DbSet<CourtWorkRequisites> CourtWorkRequisites { get; set; }
        public DbSet<InstallmentPayRequisites> InstallmentPayRequisites { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public void DisabledProxy()
        {
            base.Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<CourtDocumentScans>()
            .HasRequired<CourtGeneralInformation>(s => s.CourtGeneralInformation)
            .WithMany(g => g.CourtDocumentScans)
            .HasForeignKey<int>(s => s.CourtGeneralInformId);

            modelBuilder.Entity<CourtWorkRequisites>()
            .HasRequired<CourtGeneralInformation>(s => s.CourtGeneralInformation)
            .WithMany(g => g.CourtWorkRequisites)
            .HasForeignKey<int>(s => s.CourtGeneralInformId);
            modelBuilder.Entity<InstallmentPayRequisites>()
              .HasRequired<CourtGeneralInformation>(s => s.CourtGeneralInformation)
              .WithMany(g => g.InstallmentPayRequisites)
              .HasForeignKey<int>(s => s.CourtGeneralInformId);

            modelBuilder.Entity<CourtValueDictionary>()
          .HasRequired<CourtNameDictionary>(s => s.CourtNameDictionary)
          .WithMany(g => g.CourtValueDictionaries)
          .HasForeignKey<int>(s => s.CourtNameDictionaryId);
        }
    }
    [Table(name: "LogsIpu", Schema = "dbo")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int IdPU { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime? DateTime { get; set; }
    }
    public class Flags
    {
        [Key]
        public int Id { get; set; }
        public string NameAction { get; set; }
        public bool Flag { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
