namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentaddstartandEnddate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtInstallmentPlans", "DateStartPayment", c => c.DateTime());
            AddColumn("dbo.CourtInstallmentPlans", "DateEndPayment", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtInstallmentPlans", "DateEndPayment");
            DropColumn("dbo.CourtInstallmentPlans", "DateStartPayment");
        }
    }
}
