namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AllEntities_27Jun2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAssemblyLines",
                c => new
                    {
                        AssemblyLineId = c.Long(nullable: false, identity: true),
                        AssemblyLineName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.AssemblyLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
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
                "dbo.tblPackagingLine",
                c => new
                    {
                        PackagingLineId = c.Long(nullable: false, identity: true),
                        PackagingLineName = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PackagingLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
            CreateTable(
                "dbo.tblQualityControl",
                c => new
                    {
                        QCId = c.Long(nullable: false, identity: true),
                        QCName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QCId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
            CreateTable(
                "dbo.tblRepairLine",
                c => new
                    {
                        RepairLineId = c.Long(nullable: false, identity: true),
                        RepairLineName = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RepairLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
            CreateTable(
                "dbo.tblAssemblyLineStockDetail",
                c => new
                    {
                        ALSDetail = c.Long(nullable: false, identity: true),
                        AssemblyLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.ALSDetail);
            
            CreateTable(
                "dbo.tblAssemblyLineStockInfo",
                c => new
                    {
                        ALSInfo = c.Long(nullable: false, identity: true),
                        AssemblyLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ALSInfo);
            
            CreateTable(
                "dbo.tblFaultyItemStockDetail",
                c => new
                    {
                        FaultyItemStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        QCId = c.Long(),
                        RepairLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FaultyItemStockDetailId);
            
            CreateTable(
                "dbo.tblFaultyItemStockInfo",
                c => new
                    {
                        FaultyItemStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        QCId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FaultyItemStockInfoId);
            
            CreateTable(
                "dbo.tblFinishGoodsInfo",
                c => new
                    {
                        FinishGoodsInfoId = c.Long(nullable: false, identity: true),
                        ProductionLineId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
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
                        PackagingLineId = c.Long(nullable: false),
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
                        PackagingLineId = c.Long(),
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
                        PackagingLineId = c.Long(),
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
                "dbo.tblPackagingItemStockDetail",
                c => new
                    {
                        PItemStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        QCId = c.Long(),
                        PackagingLineId = c.Long(),
                        PackagingLineToId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PItemStockDetailId);
            
            CreateTable(
                "dbo.tblPackagignItemStockInfo",
                c => new
                    {
                        PItemStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        PackagingLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        TransferQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PItemStockInfoId);
            
            CreateTable(
                "dbo.tblPackagingLineStockDetail",
                c => new
                    {
                        PLStockDetailId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.PLStockDetailId);
            
            CreateTable(
                "dbo.tblPackagingLineStockInfo",
                c => new
                    {
                        PLStockInfoId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PLStockInfoId);
            
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
                "dbo.tblQCItemStockDetail",
                c => new
                    {
                        QCItemStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        QCId = c.Long(),
                        AssemblyLineId = c.Long(),
                        RepairLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        LabId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QCItemStockDetailId);
            
            CreateTable(
                "dbo.tblQCItemStockInfo",
                c => new
                    {
                        QCItemStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        QCId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        RepairQty = c.Int(nullable: false),
                        PackagingQty = c.Int(nullable: false),
                        LabQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QCItemStockInfoId);
            
            CreateTable(
                "dbo.tblQRCodeTrace",
                c => new
                    {
                        CodeId = c.Long(nullable: false, identity: true),
                        CodeNo = c.String(maxLength: 200),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        ColorId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ColorName = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 200),
                        ReferenceId = c.String(),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CodeId);
            
            CreateTable(
                "dbo.tblQualityControlLineStockDetail",
                c => new
                    {
                        QCStockDetailId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        AssemblyLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.QCStockDetailId);
            
            CreateTable(
                "dbo.tblQualityControlLineStockInfo",
                c => new
                    {
                        QCStockInfoId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        AssemblyLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QCStockInfoId);
            
            CreateTable(
                "dbo.tblRepairItemStockDetail",
                c => new
                    {
                        RPItemStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        QCId = c.Long(),
                        RepairLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RPItemStockDetailId);
            
            CreateTable(
                "dbo.tblRepairItemStockInfo",
                c => new
                    {
                        RPItemStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        QCId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        QCQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RPItemStockInfoId);
            
            CreateTable(
                "dbo.tblRepairLineStockDetail",
                c => new
                    {
                        RLStockDetailId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.RLStockDetailId);
            
            CreateTable(
                "dbo.tblRepairLineStockInfo",
                c => new
                    {
                        RLStockInfoId = c.Long(nullable: false, identity: true),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        ProductionLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RLStockInfoId);
            
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
                        RequisitionType = c.String(maxLength: 50),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WarehouseId = c.Long(nullable: false),
                        LineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        IsBundle = c.Boolean(nullable: false),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.ReqInfoId);
            
            CreateTable(
                "dbo.tblTransferFromQCDetail",
                c => new
                    {
                        TFQDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TSQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TFQDetailId)
                .ForeignKey("dbo.tblTransferFromQCInfo", t => t.TSQInfoId, cascadeDelete: true)
                .Index(t => t.TSQInfoId);
            
            CreateTable(
                "dbo.tblTransferFromQCInfo",
                c => new
                    {
                        TFQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        TransferFor = c.String(maxLength: 100),
                        RepairTransferReason = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.TFQInfoId);
            
            CreateTable(
                "dbo.tblTransferRepairItemToQcDetail",
                c => new
                    {
                        TRQDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TRQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TRQDetailId)
                .ForeignKey("dbo.tblTransferRepairItemToQcInfo", t => t.TRQInfoId, cascadeDelete: true)
                .Index(t => t.TRQInfoId);
            
            CreateTable(
                "dbo.tblTransferRepairItemToQcInfo",
                c => new
                    {
                        TRQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.TRQInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToAssemblyDetail",
                c => new
                    {
                        TSADetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TSAInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TSADetailId)
                .ForeignKey("dbo.tblTransferStockToAssemblyInfo", t => t.TSAInfoId, cascadeDelete: true)
                .Index(t => t.TSAInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToAssemblyInfo",
                c => new
                    {
                        TSAInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        AssemblyId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.TSAInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToPackagingLine2Detail",
                c => new
                    {
                        TP2DetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TP2InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TP2DetailId)
                .ForeignKey("dbo.tblTransferStockToPackagingLine2Info", t => t.TP2InfoId, cascadeDelete: true)
                .Index(t => t.TP2InfoId);
            
            CreateTable(
                "dbo.tblTransferStockToPackagingLine2Info",
                c => new
                    {
                        TP2InfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        PackagingLineFromId = c.Long(),
                        PackagingLineToId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TP2InfoId);
            
            CreateTable(
                "dbo.tblTransferStockToQCDetail",
                c => new
                    {
                        TSQDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TSQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TSQDetailId)
                .ForeignKey("dbo.tblTransferStockToQCInfo", t => t.TSQInfoId, cascadeDelete: true)
                .Index(t => t.TSQInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToQCInfo",
                c => new
                    {
                        TSQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        AssemblyId = c.Long(),
                        QCLineId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.TSQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferStockToQCDetail", "TSQInfoId", "dbo.tblTransferStockToQCInfo");
            DropForeignKey("dbo.tblTransferStockToPackagingLine2Detail", "TP2InfoId", "dbo.tblTransferStockToPackagingLine2Info");
            DropForeignKey("dbo.tblTransferStockToAssemblyDetail", "TSAInfoId", "dbo.tblTransferStockToAssemblyInfo");
            DropForeignKey("dbo.tblTransferRepairItemToQcDetail", "TRQInfoId", "dbo.tblTransferRepairItemToQcInfo");
            DropForeignKey("dbo.tblTransferFromQCDetail", "TSQInfoId", "dbo.tblTransferFromQCInfo");
            DropForeignKey("dbo.tblRequsitionDetails", "ReqInfoId", "dbo.tblRequsitionInfo");
            DropForeignKey("dbo.tblProductionStockDetail", "ProductionStockInfo_ProductionStockInfoId", "dbo.tblProductionStockInfo");
            DropForeignKey("dbo.tblItemReturnDetail", "IRInfoId", "dbo.tblItemReturnInfo");
            DropForeignKey("dbo.tblFinishGoodsStockDetail", "FinishGoodsStockInfo_FinishGoodsStockInfoId", "dbo.tblFinishGoodsStockInfo");
            DropForeignKey("dbo.tblFinishGoodsSendToWarehouseDetail", "SendId", "dbo.tblFinishGoodsSendToWarehouseInfo");
            DropForeignKey("dbo.tblFinishGoodsRowMaterial", "FinishGoodsInfoId", "dbo.tblFinishGoodsInfo");
            DropForeignKey("dbo.tblAssemblyLines", "ProductionLineId", "dbo.tblProductionLines");
            DropForeignKey("dbo.tblRepairLine", "ProductionLineId", "dbo.tblProductionLines");
            DropForeignKey("dbo.tblQualityControl", "ProductionLineId", "dbo.tblProductionLines");
            DropForeignKey("dbo.tblPackagingLine", "ProductionLineId", "dbo.tblProductionLines");
            DropIndex("dbo.tblTransferStockToQCDetail", new[] { "TSQInfoId" });
            DropIndex("dbo.tblTransferStockToPackagingLine2Detail", new[] { "TP2InfoId" });
            DropIndex("dbo.tblTransferStockToAssemblyDetail", new[] { "TSAInfoId" });
            DropIndex("dbo.tblTransferRepairItemToQcDetail", new[] { "TRQInfoId" });
            DropIndex("dbo.tblTransferFromQCDetail", new[] { "TSQInfoId" });
            DropIndex("dbo.tblRequsitionDetails", new[] { "ReqInfoId" });
            DropIndex("dbo.tblProductionStockDetail", new[] { "ProductionStockInfo_ProductionStockInfoId" });
            DropIndex("dbo.tblItemReturnDetail", new[] { "IRInfoId" });
            DropIndex("dbo.tblFinishGoodsStockDetail", new[] { "FinishGoodsStockInfo_FinishGoodsStockInfoId" });
            DropIndex("dbo.tblFinishGoodsSendToWarehouseDetail", new[] { "SendId" });
            DropIndex("dbo.tblFinishGoodsRowMaterial", new[] { "FinishGoodsInfoId" });
            DropIndex("dbo.tblRepairLine", new[] { "ProductionLineId" });
            DropIndex("dbo.tblQualityControl", new[] { "ProductionLineId" });
            DropIndex("dbo.tblPackagingLine", new[] { "ProductionLineId" });
            DropIndex("dbo.tblAssemblyLines", new[] { "ProductionLineId" });
            DropTable("dbo.tblTransferStockToQCInfo");
            DropTable("dbo.tblTransferStockToQCDetail");
            DropTable("dbo.tblTransferStockToPackagingLine2Info");
            DropTable("dbo.tblTransferStockToPackagingLine2Detail");
            DropTable("dbo.tblTransferStockToAssemblyInfo");
            DropTable("dbo.tblTransferStockToAssemblyDetail");
            DropTable("dbo.tblTransferRepairItemToQcInfo");
            DropTable("dbo.tblTransferRepairItemToQcDetail");
            DropTable("dbo.tblTransferFromQCInfo");
            DropTable("dbo.tblTransferFromQCDetail");
            DropTable("dbo.tblRequsitionInfo");
            DropTable("dbo.tblRequsitionDetails");
            DropTable("dbo.tblRepairLineStockInfo");
            DropTable("dbo.tblRepairLineStockDetail");
            DropTable("dbo.tblRepairItemStockInfo");
            DropTable("dbo.tblRepairItemStockDetail");
            DropTable("dbo.tblQualityControlLineStockInfo");
            DropTable("dbo.tblQualityControlLineStockDetail");
            DropTable("dbo.tblQRCodeTrace");
            DropTable("dbo.tblQCItemStockInfo");
            DropTable("dbo.tblQCItemStockDetail");
            DropTable("dbo.tblProductionStockInfo");
            DropTable("dbo.tblProductionStockDetail");
            DropTable("dbo.tblPackagingLineStockInfo");
            DropTable("dbo.tblPackagingLineStockDetail");
            DropTable("dbo.tblPackagignItemStockInfo");
            DropTable("dbo.tblPackagingItemStockDetail");
            DropTable("dbo.tblItemReturnInfo");
            DropTable("dbo.tblItemReturnDetail");
            DropTable("dbo.tblFinishGoodsStockInfo");
            DropTable("dbo.tblFinishGoodsStockDetail");
            DropTable("dbo.tblFinishGoodsSendToWarehouseInfo");
            DropTable("dbo.tblFinishGoodsSendToWarehouseDetail");
            DropTable("dbo.tblFinishGoodsRowMaterial");
            DropTable("dbo.tblFinishGoodsInfo");
            DropTable("dbo.tblFaultyItemStockInfo");
            DropTable("dbo.tblFaultyItemStockDetail");
            DropTable("dbo.tblAssemblyLineStockInfo");
            DropTable("dbo.tblAssemblyLineStockDetail");
            DropTable("dbo.tblRepairLine");
            DropTable("dbo.tblQualityControl");
            DropTable("dbo.tblPackagingLine");
            DropTable("dbo.tblProductionLines");
            DropTable("dbo.tblAssemblyLines");
        }
    }
}
