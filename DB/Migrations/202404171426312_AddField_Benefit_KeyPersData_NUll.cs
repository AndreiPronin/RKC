namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Benefit_KeyPersData_NUll : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PersData", name: "Benefit_Id", newName: "BenefitId");
            RenameIndex(table: "dbo.PersData", name: "IX_Benefit_Id", newName: "IX_BenefitId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PersData", name: "IX_BenefitId", newName: "IX_Benefit_Id");
            RenameColumn(table: "dbo.PersData", name: "BenefitId", newName: "Benefit_Id");
        }
    }
}
