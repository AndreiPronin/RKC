namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCourt2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtInstallmentPlans", "Comment", c => c.String());
            AlterColumn("dbo.CourtWriteOffs", "DateWriteOff", c => c.DateTime());
            DropColumn("dbo.CourtInstallmentPlans", "Commnet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtInstallmentPlans", "Commnet", c => c.String());
            AlterColumn("dbo.CourtWriteOffs", "DateWriteOff", c => c.String());
            DropColumn("dbo.CourtInstallmentPlans", "Comment");
        }
    }
}
