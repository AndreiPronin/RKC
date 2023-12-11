namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courtaddField08122023 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "StatusCard", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "ShareInRight", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "InSolidarityWith", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "PlaceBirth", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtGeneralInformations", "PlaceBirth");
            DropColumn("dbo.CourtGeneralInformations", "InSolidarityWith");
            DropColumn("dbo.CourtGeneralInformations", "ShareInRight");
            DropColumn("dbo.CourtGeneralInformations", "StatusCard");
        }
    }
}
