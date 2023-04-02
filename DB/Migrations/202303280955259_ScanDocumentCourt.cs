namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScanDocumentCourt : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CourtDocumentScans", name: "CourtDocumentScansId", newName: "CourtGeneralInformId");
            RenameIndex(table: "dbo.CourtDocumentScans", name: "IX_CourtDocumentScansId", newName: "IX_CourtGeneralInformId");
            AddColumn("dbo.CourtDocumentScans", "DocumentPath", c => c.String());
            DropColumn("dbo.CourtDocumentScans", "DocumentDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourtDocumentScans", "DocumentDate", c => c.DateTime());
            DropColumn("dbo.CourtDocumentScans", "DocumentPath");
            RenameIndex(table: "dbo.CourtDocumentScans", name: "IX_CourtGeneralInformId", newName: "IX_CourtDocumentScansId");
            RenameColumn(table: "dbo.CourtDocumentScans", name: "CourtGeneralInformId", newName: "CourtDocumentScansId");
        }
    }
}
