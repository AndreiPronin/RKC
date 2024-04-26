namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Court_Add_EditDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "EditDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtGeneralInformations", "EditDate");
        }
    }
}
