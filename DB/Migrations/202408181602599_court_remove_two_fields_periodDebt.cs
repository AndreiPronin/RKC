namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class court_remove_two_fields_periodDebt : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtLitigationWorks", "PeriodDebtEndCollected", c => c.DateTime());
            AddColumn("dbo.CourtLitigationWorks", "PeriodDebtInitialCollected", c => c.DateTime());
        }
    }
}
