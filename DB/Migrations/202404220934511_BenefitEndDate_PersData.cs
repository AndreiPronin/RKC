namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenefitEndDate_PersData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersData", "BenefitEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersData", "BenefitEndDate");
        }
    }
}
