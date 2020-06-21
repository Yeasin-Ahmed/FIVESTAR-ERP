namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairStock : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRepairLineStockInfo");
            DropTable("dbo.tblRepairLineStockDetail");
        }
    }
}
