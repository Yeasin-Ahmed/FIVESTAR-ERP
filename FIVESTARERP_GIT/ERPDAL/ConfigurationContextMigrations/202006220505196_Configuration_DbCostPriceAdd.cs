namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbCostPriceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "CostPrice", c => c.Double(nullable: false));
            AddColumn("dbo.tblMobilePartStockInfo", "CostPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMobilePartStockInfo", "CostPrice");
            DropColumn("dbo.tblMobilePartStockDetails", "CostPrice");
        }
    }
}
