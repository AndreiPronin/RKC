namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldRemainderAmountPaymentRestructuring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtInstallmentPlans", "RemainderAmountPaymentRestructuring", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtInstallmentPlans", "RemainderAmountPaymentRestructuring");
        }
    }
}
