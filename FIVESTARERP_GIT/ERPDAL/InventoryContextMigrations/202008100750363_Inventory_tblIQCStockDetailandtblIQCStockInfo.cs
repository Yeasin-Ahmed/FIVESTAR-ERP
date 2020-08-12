namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_tblIQCStockDetailandtblIQCStockInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblIQCStockDetails",
                c => new
                    {
                        StockDetailId = c.Long(nullable: false, identity: true),
                        IQCId = c.Long(),
                        WarehouseId = c.Long(),
                        DescriptionId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockType = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 150),
                        SupplierId = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        IQCStockInfo_StockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.tblIQCStockInfo", t => t.IQCStockInfo_StockInfoId)
                .Index(t => t.IQCStockInfo_StockInfoId);
            
            CreateTable(
                "dbo.tblIQCStockInfo",
                c => new
                    {
                        StockInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        DescriptionId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        StockType = c.String(maxLength: 150),
                        SupplierId = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StockInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblIQCStockDetails", "IQCStockInfo_StockInfoId", "dbo.tblIQCStockInfo");
            DropIndex("dbo.tblIQCStockDetails", new[] { "IQCStockInfo_StockInfoId" });
            DropTable("dbo.tblIQCStockInfo");
            DropTable("dbo.tblIQCStockDetails");
        }
    }
}
