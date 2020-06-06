namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddModelToWarehouseStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblWarehouseStockDetails", "DescriptionId", c => c.Long());
            AddColumn("dbo.tblWarehouseStockInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblWarehouseStockInfo", "DescriptionId");
            DropColumn("dbo.tblWarehouseStockDetails", "DescriptionId");
        }
    }
}
