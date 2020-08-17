namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingIMEIRepairTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblIMEITransferToRepairDetail",
                c => new
                    {
                        IMEITRDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        PackagingLineId = c.Long(nullable: false),
                        PackagingLineName = c.String(maxLength: 100),
                        QRCode = c.String(maxLength: 100),
                        IMEI = c.String(maxLength: 150),
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
                        IMEITRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.IMEITRDetailId)
                .ForeignKey("dbo.tblIMEITransferToRepairInfo", t => t.IMEITRInfoId, cascadeDelete: true)
                .Index(t => t.IMEITRInfoId);
            
            CreateTable(
                "dbo.tblIMEITransferToRepairInfo",
                c => new
                    {
                        IMEITRInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        PackagingLineId = c.Long(nullable: false),
                        PackagingLineName = c.String(maxLength: 100),
                        QRCode = c.String(maxLength: 100),
                        IMEI = c.String(maxLength: 150),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(nullable: false),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(nullable: false),
                        UpdateDate = c.DateTime(),
                        TransferId = c.Long(nullable: false),
                        TransferCode = c.String(),
                    })
                .PrimaryKey(t => t.IMEITRInfoId);
            
            CreateTable(
                "dbo.tblTransferToPackagingRepairDetail",
                c => new
                    {
                        TPRDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(nullable: false),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(nullable: false),
                        UpdateDate = c.DateTime(),
                        TPRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TPRDetailId)
                .ForeignKey("dbo.tblTransferToPackagingRepairInfo", t => t.TPRInfoId, cascadeDelete: true)
                .Index(t => t.TPRInfoId);
            
            CreateTable(
                "dbo.tblTransferToPackagingRepairInfo",
                c => new
                    {
                        TPRInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StateStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(nullable: false),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TPRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferToPackagingRepairDetail", "TPRInfoId", "dbo.tblTransferToPackagingRepairInfo");
            DropForeignKey("dbo.tblIMEITransferToRepairDetail", "IMEITRInfoId", "dbo.tblIMEITransferToRepairInfo");
            DropIndex("dbo.tblTransferToPackagingRepairDetail", new[] { "TPRInfoId" });
            DropIndex("dbo.tblIMEITransferToRepairDetail", new[] { "IMEITRInfoId" });
            DropTable("dbo.tblTransferToPackagingRepairInfo");
            DropTable("dbo.tblTransferToPackagingRepairDetail");
            DropTable("dbo.tblIMEITransferToRepairInfo");
            DropTable("dbo.tblIMEITransferToRepairDetail");
        }
    }
}
