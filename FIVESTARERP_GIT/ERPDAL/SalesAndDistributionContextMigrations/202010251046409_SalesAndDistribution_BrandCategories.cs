namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_BrandCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBrandCategories",
                c => new
                    {
                        BrandId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
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
        }
    }
}
