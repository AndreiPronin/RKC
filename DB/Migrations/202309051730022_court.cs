namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class court : DbMigration
    {
        public override void Up()
        {
           
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("IPU.IPU_COUNTERS", "DIMENSION_ID1", "IPU.DIMENSION");
            DropForeignKey("IPU.MODEL", "BRAND_ID", "IPU.BRAND");
            DropIndex("IPU.IPU_COUNTERS", new[] { "DIMENSION_ID1" });
            DropIndex("IPU.MODEL", new[] { "BRAND_ID" });
            DropTable("dbo.view_TplusIPU_OTP");
            DropTable("dbo.view_TplusIPU_GVS");
            DropTable("IPU.IPU_LIC");
            DropTable("IPU.IPU");
            DropTable("IPU.IPU_COUNTERS");
            DropTable("IPU.DIMENSION");
            DropTable("IPU.MODEL");
            DropTable("IPU.BRAND");
            DropTable("Address.AddressReadings");
            DropTable("Address.Address");
        }
    }
}
