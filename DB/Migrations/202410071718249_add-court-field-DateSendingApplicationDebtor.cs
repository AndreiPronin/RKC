namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcourtfieldDateSendingApplicationDebtor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWorks", "DateSendingApplicationDebtor", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "DateSendingApplicationDebtor");
        }
    }
}
