namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingSectionFinishGoodsStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblPackagingFaultyStockDetail",
                c => new
                    {
                        PackagingFaultyStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        PackagingLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        IsChinaFaulty = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TransferId = c.Long(),
                        TransferCode = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.PackagingFaultyStockDetailId);
            
            CreateTable(
                "dbo.tblPackagingFaultyStockInfo",
                c => new
                    {
                        PackagingFaultyStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        PackagingLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        IsChinaFaulty = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PackagingFaultyStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblPackagingFaultyStockInfo");
            DropTable("dbo.tblPackagingFaultyStockDetail");
        }
    }
}
