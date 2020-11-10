namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddCategoryAndBRandId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDescriptions", "CategoryId", c => c.Long());
            AddColumn("dbo.tblDescriptions", "BrandId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDescriptions", "BrandId");
            DropColumn("dbo.tblDescriptions", "CategoryId");
        }
    }
}
