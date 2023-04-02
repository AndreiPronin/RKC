namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequsits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourtWorkRequisites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourtGeneralInformId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Number = c.String(),
                        Suma = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformId, cascadeDelete: true)
                .Index(t => t.CourtGeneralInformId);
            
            DropColumn("dbo.CourtWorks", "DatePayOD");
            DropColumn("dbo.CourtWorks", "DatePayPeny");
            DropColumn("dbo.CourtWorks", "DatePayGP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtWorks", "DatePayGP", c => c.DateTime());
            AddColumn("dbo.CourtWorks", "DatePayPeny", c => c.DateTime());
            AddColumn("dbo.CourtWorks", "DatePayOD", c => c.DateTime());
            DropForeignKey("dbo.CourtWorkRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.CourtWorkRequisites", new[] { "CourtGeneralInformId" });
            DropTable("dbo.CourtWorkRequisites");
        }
    }
}
