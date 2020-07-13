namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QCPassTransferInfoAddDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQCPassTransferDetail",
                c => new
                    {
                        QPassDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        QCLineId = c.Long(nullable: false),
                        QCLineName = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(nullable: false),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(nullable: false),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(nullable: false),
                        ItemName = c.String(maxLength: 100),
                        UnitId = c.Long(nullable: false),
                        UnitName = c.String(maxLength: 100),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        QPassId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QPassDetailId)
                .ForeignKey("dbo.tblQCPassTransferInformation", t => t.QPassId, cascadeDelete: true)
                .Index(t => t.QPassId);
            
            CreateTable(
                "dbo.tblQCPassTransferInformation",
                c => new
                    {
                        QPassId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        QCLineId = c.Long(nullable: false),
                        QCLineName = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(nullable: false),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(nullable: false),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(nullable: false),
                        ItemName = c.String(maxLength: 100),
                        UnitId = c.Long(nullable: false),
                        UnitName = c.String(maxLength: 100),
                        StateStatus = c.String(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QPassId);
            
            AddColumn("dbo.tblProductionAssembleStockDetail", "QCLineId", c => c.Long());
            AddColumn("dbo.tblProductionAssembleStockDetail", "QCLineName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblProductionAssembleStockDetail", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblProductionAssembleStockDetail", "PackagingLineName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblProductionAssembleStockDetail", "UnitId", c => c.Long(nullable: false));
            AddColumn("dbo.tblProductionAssembleStockDetail", "UnitName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblProductionAssembleStockInfo", "UnitId", c => c.Long(nullable: false));
            AddColumn("dbo.tblProductionAssembleStockInfo", "UnitName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblQCPassTransferDetail", "QPassId", "dbo.tblQCPassTransferInformation");
            DropIndex("dbo.tblQCPassTransferDetail", new[] { "QPassId" });
            DropColumn("dbo.tblProductionAssembleStockInfo", "UnitName");
            DropColumn("dbo.tblProductionAssembleStockInfo", "UnitId");
            DropColumn("dbo.tblProductionAssembleStockDetail", "UnitName");
            DropColumn("dbo.tblProductionAssembleStockDetail", "UnitId");
            DropColumn("dbo.tblProductionAssembleStockDetail", "PackagingLineName");
            DropColumn("dbo.tblProductionAssembleStockDetail", "PackagingLineId");
            DropColumn("dbo.tblProductionAssembleStockDetail", "QCLineName");
            DropColumn("dbo.tblProductionAssembleStockDetail", "QCLineId");
            DropTable("dbo.tblQCPassTransferInformation");
            DropTable("dbo.tblQCPassTransferDetail");
        }
    }
}
