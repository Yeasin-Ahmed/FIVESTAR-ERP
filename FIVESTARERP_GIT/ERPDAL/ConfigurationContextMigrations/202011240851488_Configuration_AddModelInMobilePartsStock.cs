namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_AddModelInMobilePartsStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "DescriptionId", c => c.Long());
            AddColumn("dbo.tblMobilePartStockInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMobilePartStockInfo", "DescriptionId");
            DropColumn("dbo.tblMobilePartStockDetails", "DescriptionId");
        }
    }
}
