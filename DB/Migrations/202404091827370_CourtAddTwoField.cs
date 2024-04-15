namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourtAddTwoField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtWorks", "DeterminationDateOverpaidGP", c => c.DateTime());
            AddColumn("dbo.CourtWorks", "DateAccountingDepartment", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtWorks", "DateAccountingDepartment");
            DropColumn("dbo.CourtWorks", "DeterminationDateOverpaidGP");
        }
    }
}
