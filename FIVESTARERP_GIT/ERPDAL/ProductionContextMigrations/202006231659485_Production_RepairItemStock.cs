namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairItemStock : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRepairItemStockInfo");
            DropTable("dbo.tblRepairItemStockDetail");
        }
    }
}
