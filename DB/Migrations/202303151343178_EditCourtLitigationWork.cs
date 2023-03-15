namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCourtLitigationWork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtExecutionFSSPs", "SumApplicationAll", c => c.Double());
            AddColumn("dbo.CourtExecutionFSSPs", "SumApplicationOd", c => c.Double());
            AddColumn("dbo.CourtExecutionFSSPs", "SumApplicationPeny", c => c.Double());
            AddColumn("dbo.CourtExecutionFSSPs", "SumApplicationGp", c => c.Double());
            AlterColumn("dbo.CourtExecutionInPFs", "SumExecutionPf", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourtExecutionInPFs", "SumExecutionPf", c => c.Double(nullable: false));
            DropColumn("dbo.CourtExecutionFSSPs", "SumApplicationGp");
            DropColumn("dbo.CourtExecutionFSSPs", "SumApplicationPeny");
            DropColumn("dbo.CourtExecutionFSSPs", "SumApplicationOd");
            DropColumn("dbo.CourtExecutionFSSPs", "SumApplicationAll");
        }
    }
}
