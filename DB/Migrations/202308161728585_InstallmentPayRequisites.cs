namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentPayRequisites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstallmentPayRequisites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourtGeneralInformId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Suma = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformId, cascadeDelete: true)
                .Index(t => t.CourtGeneralInformId);
            
            DropColumn("dbo.CourtStateDuties", "DateSendReestr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtStateDuties", "DateSendReestr", c => c.DateTime());
            DropForeignKey("dbo.InstallmentPayRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.InstallmentPayRequisites", new[] { "CourtGeneralInformId" });
            DropTable("dbo.InstallmentPayRequisites");
        }
    }
}
