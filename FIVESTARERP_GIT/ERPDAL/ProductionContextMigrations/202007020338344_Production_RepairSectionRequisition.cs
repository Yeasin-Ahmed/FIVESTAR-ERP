namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairSectionRequisition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairSectionRequisitionDetail",
                c => new
                    {
                        RSRDetailId = c.Long(nullable: false, identity: true),
                        RepairLineId = c.Long(),
                        RepairLineName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(),
                        ItemName = c.String(maxLength: 100),
                        UnitId = c.Long(),
                        UnitName = c.String(maxLength: 100),
                        RequestQty = c.Int(nullable: false),
                        IssueQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        RSRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RSRDetailId)
                .ForeignKey("dbo.tblRepairSectionRequisitionInfo", t => t.RSRInfoId, cascadeDelete: true)
                .Index(t => t.RSRInfoId);
            
            CreateTable(
                "dbo.tblRepairSectionRequisitionInfo",
                c => new
                    {
                        RSRInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        ProductionFloorName = c.String(maxLength: 100),
                        RepairLineId = c.Long(),
                        RepairLineName = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(),
                        WarehouseName = c.String(maxLength: 100),
                        TotalUnitQty = c.Int(nullable: false),
                        IssueUnitQty = c.Int(nullable: false),
                        StateStatus = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        ApprovedBy = c.Long(),
                        ApprovedDate = c.DateTime(),
                        RecheckedBy = c.Long(),
                        RecheckedDate = c.DateTime(),
                        RejectedBy = c.Long(),
                        RejectedDate = c.DateTime(),
                        CanceledBy = c.Long(),
                        CanceledDate = c.DateTime(),
                        ReceivedBy = c.Long(),
                        ReceivedDate = c.DateTime(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RSRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairSectionRequisitionDetail", "RSRInfoId", "dbo.tblRepairSectionRequisitionInfo");
            DropIndex("dbo.tblRepairSectionRequisitionDetail", new[] { "RSRInfoId" });
            DropTable("dbo.tblRepairSectionRequisitionInfo");
            DropTable("dbo.tblRepairSectionRequisitionDetail");
        }
    }
}
