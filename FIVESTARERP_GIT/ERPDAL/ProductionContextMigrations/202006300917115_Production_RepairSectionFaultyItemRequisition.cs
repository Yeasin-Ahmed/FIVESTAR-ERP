namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairSectionFaultyItemRequisition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairSectionFaultyItemRequisitionDetail",
                c => new
                    {
                        RSFIRDetailId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        RepairLineId = c.Long(nullable: false),
                        RepairLineName = c.String(maxLength: 100),
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
                        FaultyQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        ReferenceNumber = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        RSFIRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RSFIRDetailId)
                .ForeignKey("dbo.tblRepairSectionFaultyItemInfo", t => t.RSFIRInfoId, cascadeDelete: true)
                .Index(t => t.RSFIRInfoId);
            
            CreateTable(
                "dbo.tblRepairSectionFaultyItemInfo",
                c => new
                    {
                        RSFIRInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        ProductionFloorId = c.Long(nullable: false),
                        ProductionFloorName = c.String(maxLength: 100),
                        RepairLineId = c.Long(nullable: false),
                        RepairLineName = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 150),
                        TotalUnit = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RSFIRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairSectionFaultyItemRequisitionDetail", "RSFIRInfoId", "dbo.tblRepairSectionFaultyItemInfo");
            DropIndex("dbo.tblRepairSectionFaultyItemRequisitionDetail", new[] { "RSFIRInfoId" });
            DropTable("dbo.tblRepairSectionFaultyItemInfo");
            DropTable("dbo.tblRepairSectionFaultyItemRequisitionDetail");
        }
    }
}
