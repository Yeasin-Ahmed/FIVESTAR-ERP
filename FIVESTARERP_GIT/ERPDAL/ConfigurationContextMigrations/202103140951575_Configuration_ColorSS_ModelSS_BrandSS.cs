namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_ColorSS_ModelSS_BrandSS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBrandSS",
                c => new
                    {
                        BrandId = c.Long(nullable: false, identity: true),
                        BrandName = c.String(),
                        Remarks = c.String(),
                        Flag = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.tblColorSS",
                c => new
                    {
                        ColorId = c.Long(nullable: false, identity: true),
                        ColorName = c.String(),
                        Remarks = c.String(),
                        Flag = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ColorId);
            
            CreateTable(
                "dbo.tblModelSS",
                c => new
                    {
                        ModelId = c.Long(nullable: false, identity: true),
                        ModelName = c.String(),
                        BrandId = c.Long(nullable: false),
                        Remarks = c.String(),
                        Flag = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ModelId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblModelSS");
            DropTable("dbo.tblColorSS");
            DropTable("dbo.tblBrandSS");
        }
    }
}
