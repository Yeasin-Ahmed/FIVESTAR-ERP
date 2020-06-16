namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QCStock : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblQualityControlLineStockInfo");
            DropTable("dbo.tblQualityControlLineStockDetail");
        }
    }
}
