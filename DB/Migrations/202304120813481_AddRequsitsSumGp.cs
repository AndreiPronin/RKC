namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequsitsSumGp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWorks", "AmountOverpaidGP", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "AmountOverpaidGP");
        }
    }
}
