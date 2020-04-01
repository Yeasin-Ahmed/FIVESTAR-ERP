namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_LindId_RepairStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairStockInfo", "LindId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairStockInfo", "LindId");
        }
    }
}
