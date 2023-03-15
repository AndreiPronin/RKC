namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCourtWork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWorks", "RequisitesSumGP", c => c.Double());
            AddColumn("dbo.CourtWorks", "RequisitesDateGP", c => c.DateTime());
            AddColumn("dbo.CourtWorks", "RequisitesNumberGP", c => c.String());
            DropColumn("dbo.CourtWorks", "RequisitesGP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtWorks", "RequisitesGP", c => c.String());
            DropColumn("dbo.CourtWorks", "RequisitesNumberGP");
            DropColumn("dbo.CourtWorks", "RequisitesDateGP");
            DropColumn("dbo.CourtWorks", "RequisitesSumGP");
        }
    }
}
