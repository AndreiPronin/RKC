namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCourtOwnerInformation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourtOwnerInformations",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        OwnerFirstName = c.String(),
                        OwnerLastName = c.String(),
                        OwnerSurname = c.String(),
                        OwnerFloor = c.String(),
                        OwnerDateBirthday = c.String(),
                        OwnerPasportDate = c.String(),
                        OwnerPasportNumber = c.String(),
                        OwnerPasportSeria = c.String(),
                        OwnerPasportIssue = c.String(),
                        OwnerInn = c.String(),
                        OwnerSnils = c.String(),
                        OwnerAddressRegister = c.String(),
                        OwnerPlaceBirth = c.String(),
                        OwnerTypeDocuments = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourtOwnerInformations", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.CourtOwnerInformations", new[] { "CourtGeneralInformationId" });
            DropTable("dbo.CourtOwnerInformations");
        }
    }
}
