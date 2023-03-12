namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Courts : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CourtBankruptcies", name: "BankruptcyId", newName: "CourtGeneralInformationId");
            RenameIndex(table: "dbo.CourtBankruptcies", name: "IX_BankruptcyId", newName: "IX_CourtGeneralInformationId");
            CreateTable(
                "dbo.CourtDocumentScans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourtDocumentScansId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtDocumentScansId, cascadeDelete: true)
                .Index(t => t.CourtDocumentScansId);
            
            CreateTable(
                "dbo.CourtExecutionInPFs",
                c => new
                    {
                        CourtGeneralInformationId = c.Int(nullable: false),
                        FioSendSpInIo = c.String(),
                        DateSendSpInIo = c.DateTime(),
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
                        SumExecutionPf = c.Double(nullable: false),
                        Comment = c.String(),
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
                        RestructuringPaymentDate = c.DateTime(),
                        AmountPaymentRestructuring = c.Double(),
                        Commnet = c.String(),
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
                        NameCourt = c.String(),
                        FioSendCourt = c.String(),
                        HowSubmitApplicationCourt = c.String(),
                        SumIskaAll = c.Double(),
                        SumOdSendCourt = c.Double(),
                        SumPenySendCourt = c.Double(),
                        SumStateDuty = c.Double(),
                        PeriodDebtBegin = c.DateTime(),
                        PeriodDebtEnd = c.DateTime(),
                        GPDetailsAmount = c.Double(),
                        GPDetailsDatePayment = c.DateTime(),
                        GPDetailsPaymentOrderNuumber = c.String(),
                        DateDecision = c.DateTime(),
                        DateEntryDecision = c.DateTime(),
                        RequestDateIl = c.DateTime(),
                        DateIssueIL = c.DateTime(),
                        NumberIl = c.String(),
                        Comment = c.String(),
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
                        DateSendReestr = c.DateTime(),
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
                        RequisitesGP = c.String(),
                        PeriodDebtBegin = c.DateTime(),
                        PeriodDebtEnd = c.DateTime(),
                        FioSendCourt = c.String(),
                        NameCourt = c.String(),
                        DateReceptionCourt = c.DateTime(),
                        DateReturnCourtSP = c.DateTime(),
                        NumberSP = c.String(),
                        DateSP = c.DateTime(),
                        SumPayAll = c.Double(),
                        SumPayOD = c.Double(),
                        DatePayOD = c.DateTime(),
                        SumPayPeny = c.Double(),
                        DatePayPeny = c.DateTime(),
                        SumPayGP = c.Double(),
                        DatePayGP = c.DateTime(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
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
                        DateWriteOff = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.CourtGeneralInformationId)
                .ForeignKey("dbo.CourtGeneralInformations", t => t.CourtGeneralInformationId)
                .Index(t => t.CourtGeneralInformationId);
            
            AddColumn("dbo.CourtBankruptcies", "BankruptcyCaseNumber", c => c.String());
            AddColumn("dbo.CourtBankruptcies", "DateDeterminationAcceptance", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "DateDeterminationCompletion", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "DateDeterminationApplication", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "SumWriteOff", c => c.Double());
            AddColumn("dbo.CourtBankruptcies", "DateWriteOffBegin", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "DateWriteOffEnd", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "WriteOffStatus", c => c.String());
            AddColumn("dbo.CourtBankruptcies", "DateWrite", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "Comment", c => c.String());
            DropColumn("dbo.CourtBankruptcies", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtBankruptcies", "Name", c => c.String());
            DropForeignKey("dbo.CourtWriteOffs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtWorks", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtStateDuties", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtLitigationWorks", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtInstallmentPlans", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtExecutionInPFs", "CourtGeneralInformationId", "dbo.CourtGeneralInformations");
            DropForeignKey("dbo.CourtDocumentScans", "CourtDocumentScansId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.CourtWriteOffs", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtWorks", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtStateDuties", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtLitigationWorks", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtInstallmentPlans", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtExecutionInPFs", new[] { "CourtGeneralInformationId" });
            DropIndex("dbo.CourtDocumentScans", new[] { "CourtDocumentScansId" });
            DropColumn("dbo.CourtBankruptcies", "Comment");
            DropColumn("dbo.CourtBankruptcies", "DateWrite");
            DropColumn("dbo.CourtBankruptcies", "WriteOffStatus");
            DropColumn("dbo.CourtBankruptcies", "DateWriteOffEnd");
            DropColumn("dbo.CourtBankruptcies", "DateWriteOffBegin");
            DropColumn("dbo.CourtBankruptcies", "SumWriteOff");
            DropColumn("dbo.CourtBankruptcies", "DateDeterminationApplication");
            DropColumn("dbo.CourtBankruptcies", "DateDeterminationCompletion");
            DropColumn("dbo.CourtBankruptcies", "DateDeterminationAcceptance");
            DropColumn("dbo.CourtBankruptcies", "BankruptcyCaseNumber");
            DropTable("dbo.CourtWriteOffs");
            DropTable("dbo.CourtWorks");
            DropTable("dbo.CourtStateDuties");
            DropTable("dbo.CourtLitigationWorks");
            DropTable("dbo.CourtInstallmentPlans");
            DropTable("dbo.CourtExecutionInPFs");
            DropTable("dbo.CourtDocumentScans");
            RenameIndex(table: "dbo.CourtBankruptcies", name: "IX_CourtGeneralInformationId", newName: "IX_BankruptcyId");
            RenameColumn(table: "dbo.CourtBankruptcies", name: "CourtGeneralInformationId", newName: "BankruptcyId");
        }
    }
}
