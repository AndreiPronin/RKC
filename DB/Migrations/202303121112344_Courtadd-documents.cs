namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Courtadddocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtDocumentScans", "CourtDocumentScansName", c => c.String());
            AddColumn("dbo.CourtDocumentScans", "DocumentDate", c => c.DateTime());
            AddColumn("dbo.CourtDocumentScans", "DocumentDateUpload", c => c.DateTime());
            AddColumn("dbo.CourtDocumentScans", "Executor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtDocumentScans", "Executor");
            DropColumn("dbo.CourtDocumentScans", "DocumentDateUpload");
            DropColumn("dbo.CourtDocumentScans", "DocumentDate");
            DropColumn("dbo.CourtDocumentScans", "CourtDocumentScansName");
        }
    }
}
