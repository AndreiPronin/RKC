namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editCourt201220232 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtExecutionFSSPs", "SatayGroundsRefusalInitiateIP", c => c.String());
            AddColumn("dbo.CourtExecutionFSSPs", "SatayGroundsRefusalInitiateIP2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtExecutionFSSPs", "SatayGroundsRefusalInitiateIP2");
            DropColumn("dbo.CourtExecutionFSSPs", "SatayGroundsRefusalInitiateIP");
        }
    }
}
