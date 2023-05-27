namespace DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbTplus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "IPU.DIMENSION",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DIMENSION_NAME = c.String(),
                        MinValue = c.Int(),
                        MaxValue = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "IPU.IPU_COUNTERS",
                c => new
                    {
                        ID_PU = c.Int(nullable: false, identity: true),
                        FACTORY_NUMBER_PU = c.String(),
                        TYPE_PU = c.String(),
                        BRAND_PU = c.String(),
                        MODEL_PU = c.String(),
                        GIS_ID_PU = c.String(),
                        SEAL_NUMBER = c.String(),
                        DESCRIPTION = c.String(),
                        CLOSE_ = c.Boolean(),
                        DATE_CHECK = c.DateTime(),
                        DATE_CHECK_NEXT = c.DateTime(),
                        DATE_INSTALL = c.DateTime(),
                        DATE_CLOSE = c.DateTime(),
                        DESCRIPTION_CLOSE = c.String(),
                        CNTR_METER_CLOSE = c.Decimal(precision: 18, scale: 2),
                        ID_USER = c.Int(),
                        TIMESTAMP = c.DateTime(),
                        INSTALLATIONDATE = c.DateTime(),
                        SEALNUMBER = c.String(),
                        TYPEOFSEAL = c.String(),
                        SEALNUMBER2 = c.String(),
                        TYPEOFSEAL2 = c.String(),
                        FULL_LIC = c.String(),
                        CHECKPOINT_DATE = c.DateTime(),
                        CHECKPOINT_READINGS = c.Double(),
                        OPERATOR_CLOSE_DATE = c.DateTime(),
                        OPERATOR_CLOSE_READINGS = c.Double(),
                        DIMENSION_ID = c.Int(),
                        DIMENSION_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID_PU)
                .ForeignKey("IPU.DIMENSION", t => t.DIMENSION_ID1)
                .Index(t => t.DIMENSION_ID1);
            
            CreateTable(
                "IPU.IPU",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ZAVOD_NUMBER_PU = c.String(),
                        BRAND_PU = c.String(),
                        MODEL_PU = c.String(),
                        GIS_ID_PU = c.String(),
                        FULL_LIC = c.String(),
                        N_LIC = c.Decimal(precision: 18, scale: 2),
                        LINK1 = c.String(),
                        TYPE_PU = c.String(),
                        FKUBSXVS = c.Decimal(precision: 18, scale: 2),
                        FKUBSXV_2 = c.Decimal(precision: 18, scale: 2),
                        FKUBSXV_3 = c.Decimal(precision: 18, scale: 2),
                        FKUBSXV_4 = c.Decimal(precision: 18, scale: 2),
                        FKUBSOT_1 = c.Decimal(precision: 18, scale: 2),
                        FKUBSOT_2 = c.Decimal(precision: 18, scale: 2),
                        FKUBSOT_3 = c.Decimal(precision: 18, scale: 2),
                        FKUBSOT_4 = c.Decimal(precision: 18, scale: 2),
                        LINK2 = c.String(),
                        NUM_CNT_LINK1 = c.Decimal(precision: 18, scale: 2),
                        NUM_CNT_LINK2 = c.Decimal(precision: 18, scale: 2),
                        ZAV_NUM_PU_ALEX = c.String(),
                        DELETE = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "IPU.IPU_LIC",
                c => new
                    {
                        ID_LIC = c.Int(nullable: false, identity: true),
                        N_LIC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FULL_LIC = c.String(),
                        FIRST_NAME = c.String(),
                        MIDDLE_NAME = c.String(),
                        LAST_NAME = c.String(),
                        STREET = c.String(),
                        HOME = c.String(),
                        FLAT = c.String(),
                        CLOSE_ = c.Boolean(),
                        TIMESTAMP = c.DateTime(nullable: false),
                        NRM5 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PU_GVS_MULTILIC_MAIN = c.Boolean(),
                        PU_GVS_MULTILIC_SLAVE = c.Boolean(),
                        FIO = c.String(),
                    })
                .PrimaryKey(t => t.ID_LIC);
            
            CreateTable(
                "dbo.view_TplusIPU_GVS",
                c => new
                    {
                        FULL_LIC = c.String(nullable: false, maxLength: 128),
                        CODE_HOUSE = c.Decimal(precision: 18, scale: 2),
                        STREET = c.String(),
                        HOME = c.String(),
                        FLAT = c.String(),
                        FIO = c.String(),
                        TYPE_PU = c.String(),
                        FACTORY_NUMBER_PU = c.String(),
                        DATE_CHECK = c.String(),
                        DATE_CHECK_NEXT = c.String(),
                        SEALNUMBER = c.String(),
                        TYPEOFSEAL = c.String(),
                        SEALNUMBER2 = c.String(),
                        TYPEOFSEAL2 = c.String(),
                        SIGN_PU = c.String(),
                        END_READINGS = c.String(),
                        NOW_READINGS = c.Int(),
                    })
                .PrimaryKey(t => t.FULL_LIC);
            
            CreateTable(
                "dbo.view_TplusIPU_OTP",
                c => new
                    {
                        FULL_LIC = c.String(nullable: false, maxLength: 128),
                        CODE_HOUSE = c.Decimal(precision: 18, scale: 2),
                        STREET = c.String(),
                        HOME = c.String(),
                        FLAT = c.String(),
                        FIO = c.String(),
                        TYPE_PU = c.String(),
                        FACTORY_NUMBER_PU = c.String(),
                        DATE_CHECK = c.String(),
                        DATE_CHECK_NEXT = c.String(),
                        SEALNUMBER = c.String(),
                        TYPEOFSEAL = c.String(),
                        SEALNUMBER2 = c.String(),
                        TYPEOFSEAL2 = c.String(),
                        SIGN_PU = c.String(),
                        END_READINGS = c.String(),
                        NOW_READINGS = c.Int(),
                    })
                .PrimaryKey(t => t.FULL_LIC);
            
        }
        
        public override void Down()
        {
            DropForeignKey("IPU.IPU_COUNTERS", "DIMENSION_ID1", "IPU.DIMENSION");
            DropIndex("IPU.IPU_COUNTERS", new[] { "DIMENSION_ID1" });
            DropTable("dbo.view_TplusIPU_OTP");
            DropTable("dbo.view_TplusIPU_GVS");
            DropTable("IPU.IPU_LIC");
            DropTable("IPU.IPU");
            DropTable("IPU.IPU_COUNTERS");
            DropTable("IPU.DIMENSION");
        }
    }
}
