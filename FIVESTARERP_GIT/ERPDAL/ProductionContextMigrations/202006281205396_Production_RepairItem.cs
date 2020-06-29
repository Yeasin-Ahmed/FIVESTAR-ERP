namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairItem",
                c => new
                    {
                        RepairItemId = c.Long(nullable: false, identity: true),
                        ReferenceNumber = c.String(maxLength: 100),
                        QRCodeId = c.Long(nullable: false),
                        QRCode = c.String(maxLength: 100),
                        ProductionFloorId = c.Long(),
                        QCLineId = c.Long(),
                        QCLineName = c.String(maxLength: 100),
                        RepairLineId = c.Long(),
                        RepairLineName = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        ModelName = c.String(maxLength: 100),
                        WarehouseId = c.Long(),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(),
                        ItemName = c.String(maxLength: 100),
                        UnitId = c.Long(),
                        UnitName = c.String(maxLength: 100),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RepairItemId);
            
            CreateTable(
                "dbo.tblRepairItemParts",
                c => new
                    {
                        RIPartsId = c.Long(nullable: false, identity: true),
                        ReferenceNumber = c.String(maxLength: 100),
                        QRCodeId = c.Long(),
                        QRCode = c.String(maxLength: 100),
                        WarehouseId = c.Long(),
                        WarehouseName = c.String(maxLength: 100),
                        ItemTypeId = c.Long(),
                        ItemTypeName = c.String(maxLength: 100),
                        ItemId = c.Long(),
                        ItemName = c.String(maxLength: 100),
                        Qty = c.Int(nullable: false),
                        UnitId = c.Long(),
                        UnitName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        RepairItemId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RIPartsId)
                .ForeignKey("dbo.tblRepairItem", t => t.RepairItemId, cascadeDelete: true)
                .Index(t => t.RepairItemId);
            
            CreateTable(
                "dbo.tblRepairItemProblem",
                c => new
                    {
                        RIProblemId = c.Long(nullable: false, identity: true),
                        ReferenceNumber = c.String(maxLength: 100),
                        QRCodeId = c.Long(nullable: false),
                        QRCode = c.String(maxLength: 100),
                        ProblemId = c.Long(nullable: false),
                        Problem = c.String(maxLength: 200),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        RepairItemId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RIProblemId)
                .ForeignKey("dbo.tblRepairItem", t => t.RepairItemId, cascadeDelete: true)
                .Index(t => t.RepairItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairItemProblem", "RepairItemId", "dbo.tblRepairItem");
            DropForeignKey("dbo.tblRepairItemParts", "RepairItemId", "dbo.tblRepairItem");
            DropIndex("dbo.tblRepairItemProblem", new[] { "RepairItemId" });
            DropIndex("dbo.tblRepairItemParts", new[] { "RepairItemId" });
            DropTable("dbo.tblRepairItemProblem");
            DropTable("dbo.tblRepairItemParts");
            DropTable("dbo.tblRepairItem");
        }
    }
}
