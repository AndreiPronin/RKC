namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnDateIo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CourtExecutionInPFs", "DateSendSpInIo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtExecutionInPFs", "DateSendSpInIo", c => c.DateTime());
        }
    }
}
