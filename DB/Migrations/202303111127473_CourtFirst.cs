namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourtFirst : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.CourtBankruptcies",
                c => new
                    {
                        BankruptcyId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BankruptcyId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.BankruptcyId)
                .Index(t => t.BankruptcyId);
            
            CreateTable(
                "dbo.CourtGeneralInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdClient = c.String(),
                        IdВuty = c.String(),
                        Lic = c.String(),
                        Region = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Home = c.String(),
                        Flat = c.String(),
                        FioDuty = c.String(),
                        Floor = c.String(),
                        ShareOfOwnership = c.String(),
                        DateBirthday = c.String(),
                        PasportData = c.String(),
                        Inn = c.String(),
                        Snils = c.String(),
                        Pensioner = c.String(),
                        ExclusionMailing = c.String(),
                        ReasonsExclusionMailing = c.String(),
                        ExclusionCourtWork = c.String(),
                        ReasonsCourtWork = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersDataDocument", "idPersData", "dbo.PersData");
            DropForeignKey("dbo.CourtBankruptcies", "BankruptcyId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.PersDataDocument", new[] { "idPersData" });
            DropIndex("dbo.CourtBankruptcies", new[] { "BankruptcyId" });
            DropTable("dbo.vw_CounterTPlus");
            DropTable("dbo.StateCalculation");
            DropTable("dbo.Reports");
            DropTable("dbo.PersonalInformations_New");
            DropTable("dbo.PersDataDocument");
            DropTable("dbo.PersData");
            DropTable("dbo.Notifications");
            DropTable("dbo.LogsPersData");
            DropTable("dbo.LogsIpu");
            DropTable("dbo.IntegrationReadings");
            DropTable("dbo.HelpСalculations");
            DropTable("dbo.Flags");
            DropTable("dbo.DPUSummaryHouses");
            DropTable("dbo.DPUHelpCalculationInstallation");
            DropTable("dbo.CourtGeneralInformations");
            DropTable("dbo.CourtBankruptcies");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
