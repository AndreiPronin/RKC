namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Court_Task_53 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtBankruptcies", "DateDecisioDeclareCitizenBankrupt", c => c.DateTime());
            AddColumn("dbo.CourtBankruptcies", "SumOd", c => c.Double());
            AddColumn("dbo.CourtBankruptcies", "SumPeny", c => c.Double());
            AddColumn("dbo.CourtBankruptcies", "SumGp", c => c.Double());
            AddColumn("dbo.CourtInstallmentPlans", "AmountRestructuringOd", c => c.Double());
            AddColumn("dbo.CourtInstallmentPlans", "AmountRestructuringPeny", c => c.Double());
            AddColumn("dbo.CourtWorks", "DateSendApplicationOnReverseGpInCourt", c => c.DateTime());
            AddColumn("dbo.CourtWriteOffs", "SumOd", c => c.Double());
            AddColumn("dbo.CourtWriteOffs", "SumPeny", c => c.Double());
            AddColumn("dbo.CourtWriteOffs", "SumGp", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWriteOffs", "SumGp");
            DropColumn("dbo.CourtWriteOffs", "SumPeny");
            DropColumn("dbo.CourtWriteOffs", "SumOd");
            DropColumn("dbo.CourtWorks", "DateSendApplicationOnReverseGpInCourt");
            DropColumn("dbo.CourtInstallmentPlans", "AmountRestructuringPeny");
            DropColumn("dbo.CourtInstallmentPlans", "AmountRestructuringOd");
            DropColumn("dbo.CourtBankruptcies", "SumGp");
            DropColumn("dbo.CourtBankruptcies", "SumPeny");
            DropColumn("dbo.CourtBankruptcies", "SumOd");
            DropColumn("dbo.CourtBankruptcies", "DateDecisioDeclareCitizenBankrupt");
        }
    }
}
