namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FinishGoods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFinishGoodsInfo",
                c => new
                    {
                        FinishGoodsInfoId = c.Long(nullable: false, identity: true),
                        ProductionLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quanity = c.Int(nullable: false),
                        ProductionType = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FinishGoodsInfoId);
            
            CreateTable(
                "dbo.tblFinishGoodsRowMaterial",
                c => new
                    {
                        FGRMId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quanity = c.Int(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        FinishGoodsInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FGRMId)
                .ForeignKey("dbo.tblFinishGoodsInfo", t => t.FinishGoodsInfoId, cascadeDelete: true)
                .Index(t => t.FinishGoodsInfoId);
            
            CreateTable(
                "dbo.tblFinishGoodsStockDetail",
                c => new
                    {
                        FinishGoodsStockDetailId = c.Long(nullable: false, identity: true),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        DescriptionId = c.Long(),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        FinishGoodsStockInfo_FinishGoodsStockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.FinishGoodsStockDetailId)
                .ForeignKey("dbo.tblFinishGoodsStockInfo", t => t.FinishGoodsStockInfo_FinishGoodsStockInfoId)
                .Index(t => t.FinishGoodsStockInfo_FinishGoodsStockInfoId);
            
            CreateTable(
                "dbo.tblFinishGoodsStockInfo",
                c => new
                    {
                        FinishGoodsStockInfoId = c.Long(nullable: false, identity: true),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        DescriptionId = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FinishGoodsStockInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblFinishGoodsStockDetail", "FinishGoodsStockInfo_FinishGoodsStockInfoId", "dbo.tblFinishGoodsStockInfo");
            DropForeignKey("dbo.tblFinishGoodsRowMaterial", "FinishGoodsInfoId", "dbo.tblFinishGoodsInfo");
            DropIndex("dbo.tblFinishGoodsStockDetail", new[] { "FinishGoodsStockInfo_FinishGoodsStockInfoId" });
            DropIndex("dbo.tblFinishGoodsRowMaterial", new[] { "FinishGoodsInfoId" });
            DropTable("dbo.tblFinishGoodsStockInfo");
            DropTable("dbo.tblFinishGoodsStockDetail");
            DropTable("dbo.tblFinishGoodsRowMaterial");
            DropTable("dbo.tblFinishGoodsInfo");
        }
    }
}
