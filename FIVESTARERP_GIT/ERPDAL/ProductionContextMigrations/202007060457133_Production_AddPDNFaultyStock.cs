namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPDNFaultyStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductionFaultyStockDetail",
                c => new
                    {
                        PDNFaultyStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        RepairLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
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
                .PrimaryKey(t => t.PDNFaultyStockDetailId);
            
            CreateTable(
                "dbo.tblProductionFaultyStockInfo",
                c => new
                    {
                        PDNFaultyStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PDNFaultyStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblProductionFaultyStockInfo");
            DropTable("dbo.tblProductionFaultyStockDetail");
        }
    }
}
