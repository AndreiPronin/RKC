namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReceiptNotSend",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lic = c.String(),
                        Email = c.String(),
                        Month = c.Int(nullable: false),
                        ErrorDescription = c.String(),
                        IsSend = c.Boolean(nullable: false),
                        TypeReceipt = c.Int(nullable: false),
                        DateTimeSend = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReceiptNotSend");
        }
    }
}
