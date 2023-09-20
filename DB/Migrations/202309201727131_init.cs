namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.CourtBankruptcies",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        BankruptcyCaseNumber = c.String(),
                        DateDeterminationAcceptance = c.DateTime(),
                        DateDeterminationCompletion = c.DateTime(),
                        DateDeterminationApplication = c.DateTime(),
                        SumWriteOff = c.Double(),
                        DateWriteOffBegin = c.DateTime(),
                        DateWriteOffEnd = c.DateTime(),
                        WriteOffStatus = c.String(),
                        DateWrite = c.DateTime(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtGeneralInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lic = c.String(),
                        Region = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Home = c.String(),
                        Flat = c.String(),
                        FioDuty = c.String(),
                        Floor = c.String(),
                        ShareOfOwnership = c.String(),
                        CadastrNumber = c.String(),
                        DateBirthday = c.String(),
                        PasportDate = c.String(),
                        PasportNumber = c.String(),
                        PasportSeria = c.String(),
                        PasportIssue = c.String(),
                        Inn = c.String(),
                        Snils = c.String(),
                        Pensioner = c.String(),
                        ExclusionMailing = c.String(),
                        ReasonsExclusionMailing = c.String(),
                        ExclusionCourtWork = c.String(),
                        ReasonsCourtWork = c.String(),
                        AddressRegister = c.String(),
                        DateDeath = c.DateTime(),
                        Comment = c.String(),
                        DateCreate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourtDocumentScans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourtGeneralInformId = c.Int(nullable: false),
                        CourtDocumentScansName = c.String(),
                        DocumentPath = c.String(),
                        DocumentDateUpload = c.DateTime(),
                        Executor = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformId, cascadeDelete: true)
                .Index(t => t.CourtGeneralInformId);
            
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
                        SumApplicationAll = c.Double(),
                        SumApplicationOd = c.Double(),
                        SumApplicationPeny = c.Double(),
                        SumApplicationGp = c.Double(),
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
                        NumberIP2 = c.String(),
                        IPInitiationDate2 = c.DateTime(),
                        SumDecisionInitateIP2 = c.Double(),
                        IPEndDate2 = c.DateTime(),
                        GroundsEndingIP2 = c.String(),
                        IPExecutionDate2 = c.DateTime(),
                        ReasonExecutionIP2 = c.String(),
                        DateReceiptOriginalIDEndIP2 = c.DateTime(),
                        DateRefusalInitiateIP2 = c.DateTime(),
                        GroundsRefusalInitiateIP2 = c.String(),
                        DateReceiptOriginalIDcaseRefusalInitiateIP2 = c.DateTime(),
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
                        DateTask = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtExecutionInPFs",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        FioSendSpInIo = c.String(),
                        ExecutiveAgency = c.String(),
                        AdresIo = c.String(),
                        DateSendApplicationIdInPf = c.DateTime(),
                        WaySendOriginalApplicationIdInPf = c.String(),
                        SumApplicationPfAll = c.Double(),
                        SumApplicationPfOd = c.Double(),
                        SumApplicationPfPeny = c.Double(),
                        SumApplicationPfGp = c.Double(),
                        DateReturnPF = c.DateTime(),
                        LengthStayDay = c.Int(),
                        DateReturnIdPF = c.DateTime(),
                        DateReturnId = c.DateTime(),
                        ReasonReturnIdPf = c.String(),
                        NumberMailPfReturnId = c.String(),
                        SumExecutionPf = c.Double(),
                        Comment = c.String(),
                        DateTask = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtInstallmentPlans",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        DateAcceptanceApplicationRestructuring = c.DateTime(),
                        AmountRestructuring = c.Double(),
                        StartingMonthRestructuring = c.DateTime(),
                        FinalMonthRestructuring = c.DateTime(),
                        AmountMonthlyRestructuringPayment = c.Double(),
                        DateStartPayment = c.DateTime(),
                        DateEndPayment = c.DateTime(),
                        AmountPaymentRestructuring = c.Double(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtLitigationWorks",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        DateDecisionCansel = c.DateTime(),
                        DateReceipt = c.DateTime(),
                        DateSubmission = c.DateTime(),
                        DateSendPirRCO = c.DateTime(),
                        AmountWithdrawnAll = c.Double(),
                        AmountWithdrawnOd = c.Double(),
                        AmountWithdrawnPeny = c.Double(),
                        AmountRecoveredExpenses = c.Double(),
                        AmountWithdrawnGp = c.Double(),
                        NameCourt = c.String(),
                        AddressCourt = c.String(),
                        FioSendCourt = c.String(),
                        HowSubmitApplicationCourt = c.String(),
                        SumIskaAll = c.Double(),
                        SumOdSendCourt = c.Double(),
                        SumPenySendCourt = c.Double(),
                        SumOtherCourt = c.Double(),
                        SumStateDuty = c.Double(),
                        PeriodDebtBegin = c.DateTime(),
                        PeriodDebtEnd = c.DateTime(),
                        DateDecision = c.DateTime(),
                        SumOverpaidGP = c.Double(),
                        SumPayGP = c.Double(),
                        DateEntryDecision = c.DateTime(),
                        RequestDateIl = c.DateTime(),
                        DateIssueIL = c.DateTime(),
                        DateFactGetIL = c.DateTime(),
                        NumberIl = c.String(),
                        Comment = c.String(),
                        DateTask = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtStateDuties",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        DateSendOnReturnFNS = c.DateTime(),
                        DateReturnFNS = c.DateTime(),
                        ReasonReturn = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            CreateTable(
                "dbo.CourtWorks",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        SumDebtNowDate = c.Double(),
                        SumDebtSendCourt = c.Double(),
                        SumOdSendCourt = c.Double(),
                        SumPenySendCourt = c.Double(),
                        SumGP = c.Double(),
                        RequisitesSumGP = c.Double(),
                        RequisitesDateGP = c.DateTime(),
                        RequisitesNumberGP = c.String(),
                        AmountOverpaidGP = c.Double(),
                        PeriodDebtBegin = c.DateTime(),
                        PeriodDebtEnd = c.DateTime(),
                        FioSendCourt = c.String(),
                        SubmitApplicationCourt = c.String(),
                        NameCourt = c.String(),
                        AddressCourt = c.String(),
                        DateReceptionCourt = c.DateTime(),
                        DateReturnCourtSP = c.DateTime(),
                        ReasonReturningApplication = c.String(),
                        NumberSP = c.String(),
                        DateSP = c.DateTime(),
                        SumPayAll = c.Double(),
                        SumPayOD = c.Double(),
                        SumPayPeny = c.Double(),
                        SumPayGP = c.Double(),
                        Comment = c.String(),
                        DateTask = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
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
            
            CreateTable(
                "dbo.CourtWriteOffs",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        DocumentsPreparedWriteOff = c.String(),
                        SumWriteOff = c.Double(),
                        DateWriteOffBegin = c.DateTime(),
                        DateWriteOffEnd = c.DateTime(),
                        WriteOffStatus = c.String(),
                        DateWriteOff = c.DateTime(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
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
            
            CreateTable(
                "dbo.LitigationWorkRequisites",
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
