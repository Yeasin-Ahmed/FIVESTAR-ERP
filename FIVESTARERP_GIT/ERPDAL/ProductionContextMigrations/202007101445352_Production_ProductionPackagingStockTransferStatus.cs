namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ProductionPackagingStockTransferStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblProductionToPackagingStockTransferInfo", "StateStatus", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblProductionToPackagingStockTransferInfo", "StateStatus");
        }
    }
}
