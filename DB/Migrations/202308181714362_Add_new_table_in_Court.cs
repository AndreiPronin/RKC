namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_new_table_in_Court : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.CourtGeneralInformations", "CadastrNumber", c => c.String());
            AddColumn("dbo.CourtLitigationWorks", "AmountRecoveredExpenses", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "SumOtherCourt", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "SumOverpaidGP", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "SumPayGP", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "DateFactGetIL", c => c.DateTime());
            DropColumn("dbo.CourtInstallmentPlans", "RestructuringPaymentDate");
            DropColumn("dbo.CourtLitigationWorks", "GPDetailsAmount");
            DropColumn("dbo.CourtLitigationWorks", "GPDetailsDatePayment");
            DropColumn("dbo.CourtLitigationWorks", "GPDetailsPaymentOrderNuumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtLitigationWorks", "GPDetailsPaymentOrderNuumber", c => c.String());
            AddColumn("dbo.CourtLitigationWorks", "GPDetailsDatePayment", c => c.DateTime());
            AddColumn("dbo.CourtLitigationWorks", "GPDetailsAmount", c => c.Double());
            AddColumn("dbo.CourtInstallmentPlans", "RestructuringPaymentDate", c => c.DateTime());
            DropForeignKey("dbo.LitigationWorkRequisites", "CourtGeneralInformId", "dbo.CourtGeneralInformations");
            DropIndex("dbo.LitigationWorkRequisites", new[] { "CourtGeneralInformId" });
            DropColumn("dbo.CourtLitigationWorks", "DateFactGetIL");
            DropColumn("dbo.CourtLitigationWorks", "SumPayGP");
            DropColumn("dbo.CourtLitigationWorks", "SumOverpaidGP");
            DropColumn("dbo.CourtLitigationWorks", "SumOtherCourt");
            DropColumn("dbo.CourtLitigationWorks", "AmountRecoveredExpenses");
            DropColumn("dbo.CourtGeneralInformations", "CadastrNumber");
            DropTable("dbo.LitigationWorkRequisites");
        }
    }
}
