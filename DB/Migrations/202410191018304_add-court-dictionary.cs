namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcourtdictionary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWriteOffs", "ReasonWriteOff", c => c.String());
            AddColumn("dbo.CourtWriteOffs", "SubjectWriteOff", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWriteOffs", "SubjectWriteOff");
            DropColumn("dbo.CourtWriteOffs", "ReasonWriteOff");
        }
    }
}
