namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_PackagingItemStock : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblPackagignItemStockInfo");
            DropTable("dbo.tblPackagingItemStockDetail");
        }
    }
}
