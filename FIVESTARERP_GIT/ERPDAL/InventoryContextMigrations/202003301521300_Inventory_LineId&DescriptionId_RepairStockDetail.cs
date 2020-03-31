namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_LineIdDescriptionId_RepairStockDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairStockDetails", "LindId", c => c.Long());
            AddColumn("dbo.tblRepairStockDetails", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairStockDetails", "DescriptionId");
            DropColumn("dbo.tblRepairStockDetails", "LindId");
        }
    }
}
