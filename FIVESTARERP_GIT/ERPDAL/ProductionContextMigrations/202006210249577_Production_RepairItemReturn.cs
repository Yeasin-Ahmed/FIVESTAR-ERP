namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairItemReturn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferRepairItemToQcDetail",
                c => new
                    {
                        TRQDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
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
                        TRQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TRQDetailId)
                .ForeignKey("dbo.tblTransferRepairItemToQcInfo", t => t.TRQInfoId, cascadeDelete: true)
                .Index(t => t.TRQInfoId);
            
            CreateTable(
                "dbo.tblTransferRepairItemToQcInfo",
                c => new
                    {
                        TRQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                    })
                .PrimaryKey(t => t.TRQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferRepairItemToQcDetail", "TRQInfoId", "dbo.tblTransferRepairItemToQcInfo");
            DropIndex("dbo.tblTransferRepairItemToQcDetail", new[] { "TRQInfoId" });
            DropTable("dbo.tblTransferRepairItemToQcInfo");
            DropTable("dbo.tblTransferRepairItemToQcDetail");
        }
    }
}
