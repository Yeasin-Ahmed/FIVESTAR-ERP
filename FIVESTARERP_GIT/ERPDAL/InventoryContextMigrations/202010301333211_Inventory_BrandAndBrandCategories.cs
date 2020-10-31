namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_BrandAndBrandCategories : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblBrandCategories", "CategoryId", "dbo.tblCategory");
            DropForeignKey("dbo.tblBrandCategories", "BrandId", "dbo.tblBrand");
            DropIndex("dbo.tblBrandCategories", new[] { "CategoryId" });
            DropIndex("dbo.tblBrandCategories", new[] { "BrandId" });
            DropTable("dbo.tblBrandCategories");
            DropTable("dbo.tblBrand");
        }
    }
}
