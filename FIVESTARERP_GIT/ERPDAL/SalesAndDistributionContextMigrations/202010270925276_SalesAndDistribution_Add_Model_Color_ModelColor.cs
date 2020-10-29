namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_Add_Model_Color_ModelColor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblColor",
                c => new
                    {
                        ColorId = c.Long(nullable: false, identity: true),
                        ColorName = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Boolean(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Boolean(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ColorId);
            
            CreateTable(
                "dbo.tblModel",
                c => new
                    {
                        ModelId = c.Long(nullable: false, identity: true),
                        ModelName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Boolean(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Boolean(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ModelId);
            
            CreateTable(
                "dbo.tblModelColors",
                c => new
                    {
                        ModelId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ModelId, t.ColorId })
                .ForeignKey("dbo.tblColor", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.tblModel", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId)
                .Index(t => t.ColorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblModelColors", "ModelId", "dbo.tblModel");
            DropForeignKey("dbo.tblModelColors", "ColorId", "dbo.tblColor");
            DropIndex("dbo.tblModelColors", new[] { "ColorId" });
            DropIndex("dbo.tblModelColors", new[] { "ModelId" });
            DropTable("dbo.tblModelColors");
            DropTable("dbo.tblModel");
            DropTable("dbo.tblColor");
        }
    }
}
