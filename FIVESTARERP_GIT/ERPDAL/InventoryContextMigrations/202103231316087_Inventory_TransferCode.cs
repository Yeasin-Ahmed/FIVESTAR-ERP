namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_TransferCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblStockTransferInfoMToM", "TransferCode", c => c.String());
            DropColumn("dbo.tblStockTransferInfoMToM", "ItemTypeId");
            DropColumn("dbo.tblStockTransferInfoMToM", "ItemId");
            DropColumn("dbo.tblStockTransferInfoMToM", "UnitId");
            DropColumn("dbo.tblStockTransferInfoMToM", "StockInQty");
            DropColumn("dbo.tblStockTransferInfoMToM", "StockOutQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblStockTransferInfoMToM", "StockOutQty", c => c.Int());
            AddColumn("dbo.tblStockTransferInfoMToM", "StockInQty", c => c.Int());
            AddColumn("dbo.tblStockTransferInfoMToM", "UnitId", c => c.Long());
            AddColumn("dbo.tblStockTransferInfoMToM", "ItemId", c => c.Long());
            AddColumn("dbo.tblStockTransferInfoMToM", "ItemTypeId", c => c.Long());
            DropColumn("dbo.tblStockTransferInfoMToM", "TransferCode");
        }
    }
}
