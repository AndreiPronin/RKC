namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumntAdressCourt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtLitigationWorks", "AddressCourt", c => c.String());
            AddColumn("dbo.CourtWorks", "AddressCourt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "AddressCourt");
            DropColumn("dbo.CourtLitigationWorks", "AddressCourt");
        }
    }
}
