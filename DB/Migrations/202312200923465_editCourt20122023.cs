namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCourt20122023 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "FirstName", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "LastName", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "Surname", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "SatyaEndingIP", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "SatyaEndingIP2", c => c.String());
            DropColumn("dbo.CourtGeneralInformations", "FioDuty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtGeneralInformations", "FioDuty", c => c.String());
            DropColumn("dbo.CourtExecutionFSSPs", "SatyaEndingIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "SatyaEndingIP");
            DropColumn("dbo.CourtGeneralInformations", "Surname");
            DropColumn("dbo.CourtGeneralInformations", "LastName");
            DropColumn("dbo.CourtGeneralInformations", "FirstName");
        }
    }
}
