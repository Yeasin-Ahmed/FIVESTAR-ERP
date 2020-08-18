namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferPackagingRepairItemToPackagingQC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferPackagingRepairItemToQcDetail",
                c => new
                    {
                        TPRQDetailId = c.Long(nullable: false, identity: true),
                        IMEI = c.String(maxLength: 100),
                        QRCode = c.String(maxLength: 100),
                        IncomingTransferId = c.Long(nullable: false),
                        IncomingTransferCode = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TPRQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TPRQDetailId)
                .ForeignKey("dbo.tblTransferPackagingRepairItemToQcInfo", t => t.TPRQInfoId, cascadeDelete: true)
                .Index(t => t.TPRQInfoId);
            
            CreateTable(
                "dbo.tblTransferPackagingRepairItemToQcInfo",
                c => new
                    {
                        TPRQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        FloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TPRQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferPackagingRepairItemToQcDetail", "TPRQInfoId", "dbo.tblTransferPackagingRepairItemToQcInfo");
            DropIndex("dbo.tblTransferPackagingRepairItemToQcDetail", new[] { "TPRQInfoId" });
            DropTable("dbo.tblTransferPackagingRepairItemToQcInfo");
            DropTable("dbo.tblTransferPackagingRepairItemToQcDetail");
        }
    }
}
