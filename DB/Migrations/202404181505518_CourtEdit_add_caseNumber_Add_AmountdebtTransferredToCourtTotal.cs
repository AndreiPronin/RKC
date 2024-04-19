namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourtEdit_add_caseNumber_Add_AmountdebtTransferredToCourtTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtLitigationWorks", "CaseNumber", c => c.String());
            AddColumn("dbo.CourtWorks", "AmountdebtTransferredToCourtTotal", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "AmountdebtTransferredToCourtTotal");
            DropColumn("dbo.CourtLitigationWorks", "CaseNumber");
        }
    }
}
