namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbStockTransferCostPriceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferDetails", "CostPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferDetails", "CostPrice");
        }
    }
}
