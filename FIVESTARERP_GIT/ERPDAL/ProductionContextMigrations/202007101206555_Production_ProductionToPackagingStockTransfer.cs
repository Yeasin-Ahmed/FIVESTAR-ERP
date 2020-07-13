namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ProductionToPackagingStockTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductionToPackagingStockTransferDetail",
                c => new
                    {
                        PTPSTDetailId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        FloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        ModelId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        PTPSTInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PTPSTDetailId)
                .ForeignKey("dbo.tblProductionToPackagingStockTransferInfo", t => t.PTPSTInfoId, cascadeDelete: true)
                .Index(t => t.PTPSTInfoId);
            
            CreateTable(
                "dbo.tblProductionToPackagingStockTransferInfo",
                c => new
                    {
                        PTPSTInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        FloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        ModelId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PTPSTInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProductionToPackagingStockTransferDetail", "PTPSTInfoId", "dbo.tblProductionToPackagingStockTransferInfo");
            DropIndex("dbo.tblProductionToPackagingStockTransferDetail", new[] { "PTPSTInfoId" });
            DropTable("dbo.tblProductionToPackagingStockTransferInfo");
            DropTable("dbo.tblProductionToPackagingStockTransferDetail");
        }
    }
}
