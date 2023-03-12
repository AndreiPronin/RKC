namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourtFSSP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourtExecutionFSSPs",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        FioSendSpIo = c.String(),
                        ExecutiveBody = c.String(),
                        AddressIO = c.String(),
                        DateSendingApplicationFSSP = c.DateTime(),
                        SendApplicationFSSP = c.String(),
                        SumApplicationPFAll = c.Double(),
                        SumApplicationPFOd = c.Double(),
                        SumApplicationPFPeny = c.Double(),
                        SumApplicationPFGp = c.Double(),
                        NumberIP = c.String(),
                        IPInitiationDate = c.DateTime(),
                        SumDecisionInitateIP = c.Double(),
                        IPEndDate = c.DateTime(),
                        GroundsEndingIP = c.String(),
                        IPExecutionDate = c.DateTime(),
                        ReasonExecutionIP = c.String(),
                        DateReceiptOriginalIDEndIP = c.DateTime(),
                        DateRefusalInitiateIP = c.DateTime(),
                        GroundsRefusalInitiateIP = c.String(),
                        DateReceiptOriginalIDcaseRefusalInitiateIP = c.DateTime(),
                        FullNameDebtorIP = c.String(),
                        IPDateBirth = c.DateTime(),
                        SnilsIp = c.String(),
                        InnIp = c.String(),
                        PasportIp = c.String(),
                        AddressIp = c.String(),
                        AccountInformation = c.String(),
                        DateActionTakenBailiffAccounts = c.DateTime(),
                        ActionTakenBailiffAccounts = c.String(),
                        InformationAboutRealRstate = c.String(),
                        DateActionTakenBailiff = c.DateTime(),
                        ActionTakenBailiff = c.String(),
                        InformationAboutVehicle = c.String(),
                        DateActionTakenBailiffVehicle = c.DateTime(),
                        ActionTakenBailiffVehicle = c.String(),
                        PhoneNumbersDebtor = c.String(),
                        IncomePension = c.Double(),
                        DateActionTakenBailiffIncome = c.DateTime(),
                        ActionTakenBailiffIncome = c.String(),
                        NameChange = c.String(),
                        DeathRegistryOfficeData = c.String(),
                        NumberInheritanceCase = c.String(),
                        FullNameNotary = c.String(),
                        MonthCheckInheritance = c.DateTime(),
                        FullNameHeir = c.String(),
                        DateActionsBailiff = c.DateTime(),
                        ActionsBailiff = c.String(),
                        SumPaymentAllFSSP = c.Double(),
                        SumPaymentODFSSP = c.Double(),
                        DatePaymentODFSSP = c.DateTime(),
                        SumPaymentPenyFSSP = c.Double(),
                        DatePaymentPenyFSSP = c.DateTime(),
                        SumPaymentGpFSSP = c.Double(),
                        DatePaymentGpFSSP = c.DateTime(),
                        DateApplication = c.DateTime(),
                        BriefAppeal = c.String(),
                        DateApplicationSubmission = c.DateTime(),
                        ApplicationSubmissionMethod = c.String(),
                        DateReasonAppealActual = c.DateTime(),
                        BriefSummaryResponseAppeal = c.String(),
                        AdditionalInformation = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourtExecutionFSSPs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.CourtExecutionFSSPs", new[] { "CourtGeneralInformationId" });
            DropTable("dbo.CourtExecutionFSSPs");
        }
    }
}
