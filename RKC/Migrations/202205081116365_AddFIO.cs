namespace RKC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFIO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FIO", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FIO");
        }
    }
}
