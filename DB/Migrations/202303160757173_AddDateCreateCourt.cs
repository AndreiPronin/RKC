namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateCreateCourt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "DateCreate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtGeneralInformations", "DateCreate");
        }
    }
}
