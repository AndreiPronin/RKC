namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Benefit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dic.Benefit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PersData", "Benefit_Id", c => c.Int());
            CreateIndex("dbo.PersData", "Benefit_Id");
            AddForeignKey("dbo.PersData", "Benefit_Id", "dic.Benefit", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersData", "Benefit_Id", "dic.Benefit");
            DropIndex("dbo.PersData", new[] { "Benefit_Id" });
            DropColumn("dbo.PersData", "Benefit_Id");
            DropTable("dic.Benefit");
        }
    }
}
