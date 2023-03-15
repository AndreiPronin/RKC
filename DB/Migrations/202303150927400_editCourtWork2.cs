namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCourtWork2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWorks", "SubmitApplicationCourt", c => c.String());
            AddColumn("dbo.CourtWorks", "ReasonReturningApplication", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "ReasonReturningApplication");
            DropColumn("dbo.CourtWorks", "SubmitApplicationCourt");
        }
    }
}
