namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Court_Add_EditDate_Null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourtGeneralInformations", "EditDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourtGeneralInformations", "EditDate", c => c.DateTime(nullable: false));
        }
    }
}
