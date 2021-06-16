namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairSectionSemiFinishStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairSectionSemiFinishStockDetails",
                c => new
                    {
                        RSSFinishDetailsId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
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
                .PrimaryKey(t => t.RSSFinishDetailsId);
            
            CreateTable(
                "dbo.tblRepairSectionSemiFinishStockInfo",
                c => new
                    {
                        RSSFinishStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        DescriptionId = c.Long(),
                        QCId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        Flag = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RSSFinishStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRepairSectionSemiFinishStockInfo");
            DropTable("dbo.tblRepairSectionSemiFinishStockDetails");
        }
    }
}
