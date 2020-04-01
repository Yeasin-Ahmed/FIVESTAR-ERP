namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_RepairStock_LindIdToLineId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairStockDetails", "LineId", c => c.Long());
            AddColumn("dbo.tblRepairStockInfo", "LineId", c => c.Long());
            DropColumn("dbo.tblRepairStockDetails", "LindId");
            DropColumn("dbo.tblRepairStockInfo", "LindId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblRepairStockInfo", "LindId", c => c.Long());
            AddColumn("dbo.tblRepairStockDetails", "LindId", c => c.Long());
            DropColumn("dbo.tblRepairStockInfo", "LineId");
            DropColumn("dbo.tblRepairStockDetails", "LineId");
        }
    }
}
