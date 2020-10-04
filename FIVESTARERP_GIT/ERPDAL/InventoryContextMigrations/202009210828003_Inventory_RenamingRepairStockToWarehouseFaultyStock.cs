namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_RenamingRepairStockToWarehouseFaultyStock : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tblRepairStockDetails", newName: "tblWarehouseFaultyStockDetails");
            RenameTable(name: "dbo.tblRepairStockInfo", newName: "tblWarehouseFaultyStockInfo");
            RenameColumn(table: "dbo.tblWarehouseFaultyStockDetails", name: "RepairStockInfo_RStockInfoId", newName: "WarehouseFaultyStockInfo_RStockInfoId");
            //RenameIndex(table: "dbo.tblWarehouseFaultyStockDetails", name: "IX_RepairStockInfo_RStockInfoId", newName: "IX_WarehouseFaultyStockInfo_RStockInfoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.tblWarehouseFaultyStockDetails", name: "IX_WarehouseFaultyStockInfo_RStockInfoId", newName: "IX_RepairStockInfo_RStockInfoId");
            RenameColumn(table: "dbo.tblWarehouseFaultyStockDetails", name: "WarehouseFaultyStockInfo_RStockInfoId", newName: "RepairStockInfo_RStockInfoId");
            RenameTable(name: "dbo.tblWarehouseFaultyStockInfo", newName: "tblRepairStockInfo");
            RenameTable(name: "dbo.tblWarehouseFaultyStockDetails", newName: "tblRepairStockDetails");
        }
    }
}
