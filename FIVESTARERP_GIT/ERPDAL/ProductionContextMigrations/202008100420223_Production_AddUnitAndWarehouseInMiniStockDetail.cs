namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddUnitAndWarehouseInMiniStockDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMiniStockTransferDetail", "WarehouseId", c => c.Long(nullable: false));
            AddColumn("dbo.tblMiniStockTransferDetail", "UnitId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMiniStockTransferDetail", "UnitId");
            DropColumn("dbo.tblMiniStockTransferDetail", "WarehouseId");
        }
    }
}
