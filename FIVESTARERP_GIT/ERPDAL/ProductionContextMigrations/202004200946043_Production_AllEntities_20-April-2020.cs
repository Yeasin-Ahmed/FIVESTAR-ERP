namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AllEntities_20April2020 : DbMigration
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
                        UnitId = c.Long(nullable: false),
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
                        UnitId = c.Long(nullable: false),
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
                "dbo.tblFinishGoodsSendToWarehouseDetail",
                c => new
                    {
                        SendDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        Flag = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        SendId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SendDetailId)
                .ForeignKey("dbo.tblFinishGoodsSendToWarehouseInfo", t => t.SendId, cascadeDelete: true)
                .Index(t => t.SendId);
            
            CreateTable(
                "dbo.tblFinishGoodsSendToWarehouseInfo",
                c => new
                    {
                        SendId = c.Long(nullable: false, identity: true),
                        LineId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        Flag = c.String(maxLength: 150),
                        StateStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SendId);
            
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
            
            CreateTable(
                "dbo.tblItemReturnDetail",
                c => new
                    {
                        IRDetailId = c.Long(nullable: false, identity: true),
                        IRCode = c.String(maxLength: 50),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        IRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.IRDetailId)
                .ForeignKey("dbo.tblItemReturnInfo", t => t.IRInfoId, cascadeDelete: true)
                .Index(t => t.IRInfoId);
            
            CreateTable(
                "dbo.tblItemReturnInfo",
                c => new
                    {
                        IRInfoId = c.Long(nullable: false, identity: true),
                        IRCode = c.String(maxLength: 50),
                        ReturnType = c.String(maxLength: 100),
                        FaultyCase = c.String(maxLength: 100),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        DescriptionId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IRInfoId);
            
            CreateTable(
                "dbo.tblProductionLines",
                c => new
                    {
                        LineId = c.Long(nullable: false, identity: true),
                        LineNumber = c.String(maxLength: 100),
                        LineIncharge = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LineId);
            
            CreateTable(
                "dbo.tblProductionStockDetail",
                c => new
                    {
                        StockDetailId = c.Long(nullable: false, identity: true),
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
                        ProductionStockInfo_ProductionStockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.tblProductionStockInfo", t => t.ProductionStockInfo_ProductionStockInfoId)
                .Index(t => t.ProductionStockInfo_ProductionStockInfoId);
            
            CreateTable(
                "dbo.tblProductionStockInfo",
                c => new
                    {
                        ProductionStockInfoId = c.Long(nullable: false, identity: true),
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
                .PrimaryKey(t => t.ProductionStockInfoId);
            
            CreateTable(
                "dbo.tblRequsitionDetails",
                c => new
                    {
                        ReqDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ReqInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqDetailId)
                .ForeignKey("dbo.tblRequsitionInfo", t => t.ReqInfoId, cascadeDelete: true)
                .Index(t => t.ReqInfoId);
            
            CreateTable(
                "dbo.tblRequsitionInfo",
                c => new
                    {
                        ReqInfoId = c.Long(nullable: false, identity: true),
                        ReqInfoCode = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WarehouseId = c.Long(nullable: false),
                        LineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequsitionDetails", "ReqInfoId", "dbo.tblRequsitionInfo");
            DropForeignKey("dbo.tblProductionStockDetail", "ProductionStockInfo_ProductionStockInfoId", "dbo.tblProductionStockInfo");
            DropForeignKey("dbo.tblItemReturnDetail", "IRInfoId", "dbo.tblItemReturnInfo");
            DropForeignKey("dbo.tblFinishGoodsStockDetail", "FinishGoodsStockInfo_FinishGoodsStockInfoId", "dbo.tblFinishGoodsStockInfo");
            DropForeignKey("dbo.tblFinishGoodsSendToWarehouseDetail", "SendId", "dbo.tblFinishGoodsSendToWarehouseInfo");
            DropForeignKey("dbo.tblFinishGoodsRowMaterial", "FinishGoodsInfoId", "dbo.tblFinishGoodsInfo");
            DropIndex("dbo.tblRequsitionDetails", new[] { "ReqInfoId" });
            DropIndex("dbo.tblProductionStockDetail", new[] { "ProductionStockInfo_ProductionStockInfoId" });
            DropIndex("dbo.tblItemReturnDetail", new[] { "IRInfoId" });
            DropIndex("dbo.tblFinishGoodsStockDetail", new[] { "FinishGoodsStockInfo_FinishGoodsStockInfoId" });
            DropIndex("dbo.tblFinishGoodsSendToWarehouseDetail", new[] { "SendId" });
            DropIndex("dbo.tblFinishGoodsRowMaterial", new[] { "FinishGoodsInfoId" });
            DropTable("dbo.tblRequsitionInfo");
            DropTable("dbo.tblRequsitionDetails");
            DropTable("dbo.tblProductionStockInfo");
            DropTable("dbo.tblProductionStockDetail");
            DropTable("dbo.tblProductionLines");
            DropTable("dbo.tblItemReturnInfo");
            DropTable("dbo.tblItemReturnDetail");
            DropTable("dbo.tblFinishGoodsStockInfo");
            DropTable("dbo.tblFinishGoodsStockDetail");
            DropTable("dbo.tblFinishGoodsSendToWarehouseInfo");
            DropTable("dbo.tblFinishGoodsSendToWarehouseDetail");
            DropTable("dbo.tblFinishGoodsRowMaterial");
            DropTable("dbo.tblFinishGoodsInfo");
        }
    }
}
