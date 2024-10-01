namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class court_add_two_fields_periodDebt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtLitigationWorks", "PeriodDebtInitialCollected", c => c.DateTime());
            AddColumn("dbo.CourtLitigationWorks", "PeriodDebtEndCollected", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtLitigationWorks", "PeriodDebtEndCollected");
            DropColumn("dbo.CourtLitigationWorks", "PeriodDebtInitialCollected");
        }
    }
}
