namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddStockItemReturn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblStockItemReturnDetail",
                c => new
                    {
                        SIRDetailId = c.Long(nullable: false, identity: true),
                        ReturnCode = c.String(maxLength: 50),
                        DescriptionId = c.Long(nullable: false),
                        ProductionFloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(),
                        RepairLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Flag = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        SIRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SIRDetailId)
                .ForeignKey("dbo.tblStockItemReturnInfo", t => t.SIRInfoId, cascadeDelete: true)
                .Index(t => t.SIRInfoId);
            
            CreateTable(
                "dbo.tblStockItemReturnInfo",
                c => new
                    {
                        SIRInfoId = c.Long(nullable: false, identity: true),
                        ReturnCode = c.String(maxLength: 50),
                        DescriptionId = c.Long(nullable: false),
                        ProductionFloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(),
                        RepairLineId = c.Long(),
                        PackagingLineId = c.Long(),
                        WarehouseId = c.Long(nullable: false),
                        Flag = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        StateStatus = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SIRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblStockItemReturnDetail", "SIRInfoId", "dbo.tblStockItemReturnInfo");
            DropIndex("dbo.tblStockItemReturnDetail", new[] { "SIRInfoId" });
            DropTable("dbo.tblStockItemReturnInfo");
            DropTable("dbo.tblStockItemReturnDetail");
        }
    }
}
