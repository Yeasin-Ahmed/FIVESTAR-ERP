namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_BrandCategories_UpdateUserEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblBrandCategories", "EUserId", c => c.Long());
            AddColumn("dbo.tblBrandCategories", "UpUserId", c => c.Long());
            DropColumn("dbo.tblBrandCategories", "EntryUser");
            DropColumn("dbo.tblBrandCategories", "UpdateBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblBrandCategories", "UpdateBy", c => c.String(maxLength: 50));
            AddColumn("dbo.tblBrandCategories", "EntryUser", c => c.String(maxLength: 50));
            DropColumn("dbo.tblBrandCategories", "UpUserId");
            DropColumn("dbo.tblBrandCategories", "EUserId");
        }
    }
}
