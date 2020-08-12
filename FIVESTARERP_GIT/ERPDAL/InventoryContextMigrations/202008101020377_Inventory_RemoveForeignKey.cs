namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_RemoveForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId", "dbo.tblIQCStockInfo");
            DropIndex("dbo.tblIQCStockDetails", new[] { "IQCStockInfo_StockInfoId" });
            DropColumn("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId", c => c.Long());
            CreateIndex("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId");
            AddForeignKey("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId", "dbo.tblIQCStockInfo", "StockInfoId");
        }
    }
}
