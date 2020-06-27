namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FaultyItemStock : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFaultyItemStockInfo");
            DropTable("dbo.tblFaultyItemStockDetail");
        }
    }
}
