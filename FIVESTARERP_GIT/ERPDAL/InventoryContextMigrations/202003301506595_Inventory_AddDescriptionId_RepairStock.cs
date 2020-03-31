namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddDescriptionId_RepairStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairStockInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairStockInfo", "DescriptionId");
        }
    }
}
