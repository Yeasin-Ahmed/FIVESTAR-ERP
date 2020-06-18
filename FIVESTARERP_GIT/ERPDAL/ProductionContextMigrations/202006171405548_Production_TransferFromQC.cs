namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferFromQC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferFromQCDetail",
                c => new
                    {
                        TFQDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TSQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TFQDetailId)
                .ForeignKey("dbo.tblTransferFromQCInfo", t => t.TSQInfoId, cascadeDelete: true)
                .Index(t => t.TSQInfoId);
            
            CreateTable(
                "dbo.tblTransferFromQCInfo",
                c => new
                    {
                        TFQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        TransferFor = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TFQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferFromQCDetail", "TSQInfoId", "dbo.tblTransferFromQCInfo");
            DropIndex("dbo.tblTransferFromQCDetail", new[] { "TSQInfoId" });
            DropTable("dbo.tblTransferFromQCInfo");
            DropTable("dbo.tblTransferFromQCDetail");
        }
    }
}
