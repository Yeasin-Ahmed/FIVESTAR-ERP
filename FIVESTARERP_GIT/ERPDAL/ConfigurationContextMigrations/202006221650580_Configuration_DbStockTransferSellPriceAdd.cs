namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbStockTransferSellPriceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "SellPrice", c => c.Double(nullable: false));
            AddColumn("dbo.tblMobilePartStockInfo", "SellPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMobilePartStockInfo", "SellPrice");
            DropColumn("dbo.tblMobilePartStockDetails", "SellPrice");
        }
    }
}
