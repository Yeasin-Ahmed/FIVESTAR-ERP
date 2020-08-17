namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_PackagingRepairStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblPackagingRepairItemStockDetail",
                c => new
                    {
                        PRIStockDetailId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(),
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
                    })
                .PrimaryKey(t => t.PRIStockDetailId);
            
            CreateTable(
                "dbo.tblPackagingRepairItemStockInfo",
                c => new
                    {
                        PRIStockInfoId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(),
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
                .PrimaryKey(t => t.PRIStockInfoId);
            
            CreateTable(
                "dbo.tblPackagingRepairRawStockDetail",
                c => new
                    {
                        PRRStockDetailId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(),
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
                    })
                .PrimaryKey(t => t.PRRStockDetailId);
            
            CreateTable(
                "dbo.tblPackagingRepairRawStockInfo",
                c => new
                    {
                        PRRStockInfoId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(),
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
                .PrimaryKey(t => t.PRRStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblPackagingRepairRawStockInfo");
            DropTable("dbo.tblPackagingRepairRawStockDetail");
            DropTable("dbo.tblPackagingRepairItemStockInfo");
            DropTable("dbo.tblPackagingRepairItemStockDetail");
        }
    }
}
