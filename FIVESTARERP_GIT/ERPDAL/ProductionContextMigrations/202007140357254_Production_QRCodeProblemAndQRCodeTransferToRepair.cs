namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QRCodeProblemAndQRCodeTransferToRepair : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQRCodeProblem",
                c => new
                    {
                        QRProbId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(maxLength: 100),
                        AssemblyLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        ProblemId = c.Long(nullable: false),
                        ProblemName = c.String(maxLength: 250),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        QRTRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QRProbId)
                .ForeignKey("dbo.tblQRCodeTransferToRepairInfo", t => t.QRTRInfoId, cascadeDelete: true)
                .Index(t => t.QRTRInfoId);
            
            CreateTable(
                "dbo.tblQRCodeTransferToRepairInfo",
                c => new
                    {
                        QRTRInfoId = c.Long(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.QRTRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblQRCodeProblem", "QRTRInfoId", "dbo.tblQRCodeTransferToRepairInfo");
            DropIndex("dbo.tblQRCodeProblem", new[] { "QRTRInfoId" });
            DropTable("dbo.tblQRCodeTransferToRepairInfo");
            DropTable("dbo.tblQRCodeProblem");
        }
    }
}
