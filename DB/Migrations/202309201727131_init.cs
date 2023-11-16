namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {

           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersDataDocument", "idPersData", "dbo.PersData");
            DropForeignKey("dic.CourtValueDictionary", "CourtNameDictionaryId", "dic.CourtNameDictionary");
            DropForeignKey("dbo.CourtBankruptcies", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.LitigationWorkRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.InstallmentPayRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtWriteOffs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtWorkRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtWorks", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtStateDuties", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtLitigationWorks", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtInstallmentPlans", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtExecutionInPFs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtExecutionFSSPs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtDocumentScans", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.PersDataDocument", new[] { "idPersData" });
            DropIndex("dic.CourtValueDictionary", new[] { "CourtNameDictionaryId" });
            DropIndex("dbo.LitigationWorkRequisites", new[] { "CourtGeneralInformId" });
            DropIndex("dbo.InstallmentPayRequisites", new[] { "CourtGeneralInformId" });
            DropIndex("dbo.CourtWriteOffs", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtWorkRequisites", new[] { "CourtGeneralInformId" });
            DropIndex("dbo.CourtWorks", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtStateDuties", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtLitigationWorks", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtInstallmentPlans", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtExecutionInPFs", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtExecutionFSSPs", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtDocumentScans", new[] { "CourtGeneralInformId" });
            DropIndex("dbo.CourtBankruptcies", new[] { "CourtGeneralInformationId" });
            DropTable("dbo.vw_CounterTPlus");
            DropTable("dbo.StateCalculation");
            DropTable("dbo.Reports");
            DropTable("dbo.PersonalInformations_New");
            DropTable("dbo.PersDataDocument");
            DropTable("dbo.PersData");
            DropTable("dbo.Notifications");
            DropTable("dbo.LogsPersData");
            DropTable("dbo.LogsIpu");
            DropTable("dbo.IntegrationReadings");
            DropTable("dbo.HelpСalculations");
            DropTable("dbo.Flags");
            DropTable("dic.CourtValueDictionary");
            DropTable("dic.CourtNameDictionary");
            DropTable("dbo.LitigationWorkRequisites");
            DropTable("dbo.InstallmentPayRequisites");
            DropTable("dbo.CourtWriteOffs");
            DropTable("dbo.CourtWorkRequisites");
            DropTable("dbo.CourtWorks");
            DropTable("dbo.CourtStateDuties");
            DropTable("dbo.CourtLitigationWorks");
            DropTable("dbo.CourtInstallmentPlans");
            DropTable("dbo.CourtExecutionInPFs");
            DropTable("dbo.CourtExecutionFSSPs");
            DropTable("dbo.CourtDocumentScans");
            DropTable("dbo.CourtGeneralInformations");
            DropTable("dbo.CourtBankruptcies");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
