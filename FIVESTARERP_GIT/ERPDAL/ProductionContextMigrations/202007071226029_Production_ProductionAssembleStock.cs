namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ProductionAssembleStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductionAssembleStockDetail",
                c => new
                    {
                        PASDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(nullable: false),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(nullable: false),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(nullable: false),
                        ItemName = c.String(maxLength: 100),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PASDetailId);
            
            CreateTable(
                "dbo.tblProductionAssembleStockInfo",
                c => new
                    {
                        PASInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(nullable: false),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(nullable: false),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(nullable: false),
                        ItemName = c.String(maxLength: 100),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PASInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblProductionAssembleStockInfo");
            DropTable("dbo.tblProductionAssembleStockDetail");
        }
    }
}
