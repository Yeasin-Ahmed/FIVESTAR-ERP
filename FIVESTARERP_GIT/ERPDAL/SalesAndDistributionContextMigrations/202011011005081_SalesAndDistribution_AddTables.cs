namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBrand",
                c => new
                    {
                        BrandId = c.Long(nullable: false, identity: true),
                        BrandName = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.tblBrandCategories",
                c => new
                    {
                        BrandId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.BrandId, t.CategoryId })
                .ForeignKey("dbo.tblBrand", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.tblCategory", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.BrandId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.tblCategory",
                c => new
                    {
                        CategoryId = c.Long(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.tblBTRCApprovedIMEI",
                c => new
                    {
                        IMEIId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        IMEI = c.String(maxLength: 200),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IMEIId);
            
            CreateTable(
                "dbo.tblColors",
                c => new
                    {
                        ColorId = c.Long(nullable: false, identity: true),
                        ColorName = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ColorId);
            
            CreateTable(
                "dbo.tblDealer",
                c => new
                    {
                        DealerId = c.Long(nullable: false, identity: true),
                        DealerName = c.String(maxLength: 200),
                        Address = c.String(maxLength: 300),
                        TelephoneNo = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 100),
                        Email = c.String(maxLength: 200),
                        ContactPersonName = c.String(maxLength: 150),
                        ContactPersonMobile = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DealerId);
            
            CreateTable(
                "dbo.tblDescriptions",
                c => new
                    {
                        DescriptionId = c.Long(nullable: false, identity: true),
                        DescriptionName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DescriptionId);
            
            CreateTable(
                "dbo.tblItemStock",
                c => new
                    {
                        StockId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        CategoryId = c.Long(),
                        BrandId = c.Long(),
                        ModelId = c.Long(nullable: false),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ColorId = c.Long(),
                        IMEI = c.String(maxLength: 100),
                        AllIMEI = c.String(maxLength: 100),
                        StockStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StockId);
            
            CreateTable(
                "dbo.tblModelColors",
                c => new
                    {
                        DescriptionId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.DescriptionId, t.ColorId })
                .ForeignKey("dbo.tblColors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.tblDescriptions", t => t.DescriptionId, cascadeDelete: true)
                .Index(t => t.DescriptionId)
                .Index(t => t.ColorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblModelColors", "DescriptionId", "dbo.tblDescriptions");
            DropForeignKey("dbo.tblModelColors", "ColorId", "dbo.tblColors");
            DropForeignKey("dbo.tblBrandCategories", "CategoryId", "dbo.tblCategory");
            DropForeignKey("dbo.tblBrandCategories", "BrandId", "dbo.tblBrand");
            DropIndex("dbo.tblModelColors", new[] { "ColorId" });
            DropIndex("dbo.tblModelColors", new[] { "DescriptionId" });
            DropIndex("dbo.tblBrandCategories", new[] { "CategoryId" });
            DropIndex("dbo.tblBrandCategories", new[] { "BrandId" });
            DropTable("dbo.tblModelColors");
            DropTable("dbo.tblItemStock");
            DropTable("dbo.tblDescriptions");
            DropTable("dbo.tblDealer");
            DropTable("dbo.tblColors");
            DropTable("dbo.tblBTRCApprovedIMEI");
            DropTable("dbo.tblCategory");
            DropTable("dbo.tblBrandCategories");
            DropTable("dbo.tblBrand");
        }
    }
}
