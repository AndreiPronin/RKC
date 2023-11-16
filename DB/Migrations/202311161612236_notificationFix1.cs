namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationFix1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiptNotSend", "NumberAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReceiptNotSend", "NumberAttempts");
        }
    }
}
