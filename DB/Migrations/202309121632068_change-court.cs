namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changecourt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtExecutionFSSPs", "NumberIP2", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "IPInitiationDate2", c => c.DateTime());
            AddColumn("dbo.CourtExecutionFSSPs", "SumDecisionInitateIP2", c => c.Double());
            AddColumn("dbo.CourtExecutionFSSPs", "IPEndDate2", c => c.DateTime());
            AddColumn("dbo.CourtExecutionFSSPs", "GroundsEndingIP2", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "IPExecutionDate2", c => c.DateTime());
            AddColumn("dbo.CourtExecutionFSSPs", "ReasonExecutionIP2", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "DateReceiptOriginalIDEndIP2", c => c.DateTime());
            AddColumn("dbo.CourtExecutionFSSPs", "DateRefusalInitiateIP2", c => c.DateTime());
            AddColumn("dbo.CourtExecutionFSSPs", "GroundsRefusalInitiateIP2", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "DateReceiptOriginalIDcaseRefusalInitiateIP2", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtExecutionFSSPs", "DateReceiptOriginalIDcaseRefusalInitiateIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "GroundsRefusalInitiateIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "DateRefusalInitiateIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "DateReceiptOriginalIDEndIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "ReasonExecutionIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "IPExecutionDate2");
            DropColumn("dbo.CourtExecutionFSSPs", "GroundsEndingIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "IPEndDate2");
            DropColumn("dbo.CourtExecutionFSSPs", "SumDecisionInitateIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "IPInitiationDate2");
            DropColumn("dbo.CourtExecutionFSSPs", "NumberIP2");
        }
    }
}
