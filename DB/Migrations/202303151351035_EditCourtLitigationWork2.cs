namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCourtLitigationWork2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtLitigationWorks", "DateSendPirRCO", c => c.DateTime());
            AddColumn("dbo.CourtLitigationWorks", "AmountWithdrawnAll", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "AmountWithdrawnOd", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "AmountWithdrawnPeny", c => c.Double());
            AddColumn("dbo.CourtLitigationWorks", "AmountWithdrawnGp", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtLitigationWorks", "AmountWithdrawnGp");
            DropColumn("dbo.CourtLitigationWorks", "AmountWithdrawnPeny");
            DropColumn("dbo.CourtLitigationWorks", "AmountWithdrawnOd");
            DropColumn("dbo.CourtLitigationWorks", "AmountWithdrawnAll");
            DropColumn("dbo.CourtLitigationWorks", "DateSendPirRCO");
        }
    }
}
