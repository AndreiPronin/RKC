namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11092023_Court : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourtGeneralInformations", "AddressRegister", c => c.String());
            AddColumn("dbo.CourtGeneralInformations", "DateDeath", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourtGeneralInformations", "DateDeath");
            DropColumn("dbo.CourtGeneralInformations", "AddressRegister");
        }
    }
}
