namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddCategoryBrandInItemStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemStock", "CategoryId", c => c.Long());
            AddColumn("dbo.tblItemStock", "BrandId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemStock", "BrandId");
            DropColumn("dbo.tblItemStock", "CategoryId");
        }
    }
}
