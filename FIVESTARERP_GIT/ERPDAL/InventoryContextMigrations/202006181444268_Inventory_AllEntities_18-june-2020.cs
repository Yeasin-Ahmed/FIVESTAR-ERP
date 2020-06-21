namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AllEntities_18june2020 : DbMigration
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
                "dbo.tblItemPreparationDetail",
                c => new
                    {
                        PreparationDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        PreparationInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PreparationDetailId)
                .ForeignKey("dbo.tblItemPreparationInfo", t => t.PreparationInfoId, cascadeDelete: true)
                .Index(t => t.PreparationInfoId);
            
            CreateTable(
                "dbo.tblItemPreparationInfo",
                c => new
                    {
                        PreparationInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PreparationInfoId);
            
            CreateTable(
                "dbo.tblItems",
                c => new
                    {
                        ItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        ItemCode = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.tblItemTypes", t => t.ItemTypeId, cascadeDelete: true)
                .Index(t => t.ItemTypeId);
            
            CreateTable(
                "dbo.tblItemTypes",
                c => new
                    {
                        ItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 100),
                        ShortName = c.String(maxLength: 5),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WarehouseId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.tblWarehouses", t => t.WarehouseId, cascadeDelete: true)
                .Index(t => t.WarehouseId);
            
            CreateTable(
                "dbo.tblWarehouses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WarehouseName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblRepairStockDetails",
                c => new
                    {
                        RStockDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        LineId = c.Long(),
                        DescriptionId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ReturnType = c.String(maxLength: 50),
                        FaultyCase = c.String(maxLength: 50),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        RepairStockInfo_RStockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.RStockDetailId)
                .ForeignKey("dbo.tblRepairStockInfo", t => t.RepairStockInfo_RStockInfoId)
                .Index(t => t.RepairStockInfo_RStockInfoId);
            
            CreateTable(
                "dbo.tblRepairStockInfo",
                c => new
                    {
                        RStockInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        LineId = c.Long(),
                        DescriptionId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RStockInfoId);
            
            CreateTable(
                "dbo.tblSupplier",
                c => new
                    {
                        SupplierId = c.Long(nullable: false, identity: true),
                        SupplierName = c.String(maxLength: 150),
                        Email = c.String(maxLength: 150),
                        Address = c.String(maxLength: 150),
                        PhoneNumber = c.String(maxLength: 50),
                        MobileNumber = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            CreateTable(
                "dbo.tblUnits",
                c => new
                    {
                        UnitId = c.Long(nullable: false, identity: true),
                        UnitName = c.String(maxLength: 100),
                        UnitSymbol = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UnitId);
            
            CreateTable(
                "dbo.tblWarehouseStockDetails",
                c => new
                    {
                        StockDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        DescriptionId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        OrderQty = c.Int(nullable: false),
                        SupplierId = c.Long(),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        WarehouseStockInfo_StockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.tblWarehouseStockInfo", t => t.WarehouseStockInfo_StockInfoId)
                .Index(t => t.WarehouseStockInfo_StockInfoId);
            
            CreateTable(
                "dbo.tblWarehouseStockInfo",
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
            DropForeignKey("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId", "dbo.tblWarehouseStockInfo");
            DropForeignKey("dbo.tblRepairStockDetails", "RepairStockInfo_RStockInfoId", "dbo.tblRepairStockInfo");
            DropForeignKey("dbo.tblItems", "ItemTypeId", "dbo.tblItemTypes");
            DropForeignKey("dbo.tblItemTypes", "WarehouseId", "dbo.tblWarehouses");
            DropForeignKey("dbo.tblItemPreparationDetail", "PreparationInfoId", "dbo.tblItemPreparationInfo");
            DropIndex("dbo.tblWarehouseStockDetails", new[] { "WarehouseStockInfo_StockInfoId" });
            DropIndex("dbo.tblRepairStockDetails", new[] { "RepairStockInfo_RStockInfoId" });
            DropIndex("dbo.tblItemTypes", new[] { "WarehouseId" });
            DropIndex("dbo.tblItems", new[] { "ItemTypeId" });
            DropIndex("dbo.tblItemPreparationDetail", new[] { "PreparationInfoId" });
            DropTable("dbo.tblWarehouseStockInfo");
            DropTable("dbo.tblWarehouseStockDetails");
            DropTable("dbo.tblUnits");
            DropTable("dbo.tblSupplier");
            DropTable("dbo.tblRepairStockInfo");
            DropTable("dbo.tblRepairStockDetails");
            DropTable("dbo.tblWarehouses");
            DropTable("dbo.tblItemTypes");
            DropTable("dbo.tblItems");
            DropTable("dbo.tblItemPreparationInfo");
            DropTable("dbo.tblItemPreparationDetail");
            DropTable("dbo.tblDescriptions");
        }
    }
}
