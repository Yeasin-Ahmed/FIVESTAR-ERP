namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AllEntities_31Mar2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDescriptions",
                c => new
                    {
                        DescriptionId = c.Long(nullable: false, identity: true),
                        DescriptionName = c.String(),
                        SubCategoryId = c.Long(),
                        Remarks = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DescriptionId);
            
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
            DropIndex("dbo.tblRequsitionDetails", new[] { "ReqInfoId" });
            DropIndex("dbo.tblProductionStockDetail", new[] { "ProductionStockInfo_ProductionStockInfoId" });
            DropIndex("dbo.tblItemReturnDetail", new[] { "IRInfoId" });
            DropTable("dbo.tblRequsitionInfo");
            DropTable("dbo.tblRequsitionDetails");
            DropTable("dbo.tblProductionStockInfo");
            DropTable("dbo.tblProductionStockDetail");
            DropTable("dbo.tblProductionLines");
            DropTable("dbo.tblItemReturnInfo");
            DropTable("dbo.tblItemReturnDetail");
            DropTable("dbo.tblDescriptions");
        }
    }
}