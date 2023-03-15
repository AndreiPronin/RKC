namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasportDataCourt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "PasportDate", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "PasportNumber", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "PasportSeria", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "PasportIssue", c => c.String());
            DropColumn("dbo.CourtGeneralInformations", "IdClient");
            DropColumn("dbo.CourtGeneralInformations", "IdВuty");
            DropColumn("dbo.CourtGeneralInformations", "PasportData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtGeneralInformations", "PasportData", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "IdВuty", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "IdClient", c => c.String());
            DropColumn("dbo.CourtGeneralInformations", "PasportIssue");
            DropColumn("dbo.CourtGeneralInformations", "PasportSeria");
            DropColumn("dbo.CourtGeneralInformations", "PasportNumber");
            DropColumn("dbo.CourtGeneralInformations", "PasportDate");
        }
    }
}
