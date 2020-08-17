namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_Add_Column_StockStatus_In_tblIQCReqDetailList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIQCStockDetails", "StockStatus", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblIQCStockDetails", "StockStatus");
        }
    }
}
