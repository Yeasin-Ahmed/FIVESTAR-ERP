namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairSectionSemiFinishTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairSectionSemiFinishTransferDetails",
                c => new
                    {
                        TransferDetailsId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(maxLength: 100),
                        AssemblyLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TransferInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TransferDetailsId)
                .ForeignKey("dbo.tblRepairSectionSemiFinishTransferInfo", t => t.TransferInfoId, cascadeDelete: true)
                .Index(t => t.TransferInfoId);
            
            CreateTable(
                "dbo.tblRepairSectionSemiFinishTransferInfo",
                c => new
                    {
                        TransferInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        StateStatus = c.String(),
                        FloorId = c.Long(),
                        QCLineId = c.Long(),
                        RepairLineId = c.Long(),
                        AssemblyLineId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        Qty = c.Int(nullable: false),
                        Remarks = c.String(),
                        Flag = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransferInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairSectionSemiFinishTransferDetails", "TransferInfoId", "dbo.tblRepairSectionSemiFinishTransferInfo");
            DropIndex("dbo.tblRepairSectionSemiFinishTransferDetails", new[] { "TransferInfoId" });
            DropTable("dbo.tblRepairSectionSemiFinishTransferInfo");
            DropTable("dbo.tblRepairSectionSemiFinishTransferDetails");
        }
    }
}
