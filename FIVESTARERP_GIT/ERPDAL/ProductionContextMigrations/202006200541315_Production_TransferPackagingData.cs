namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferPackagingData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferStockToPackagingLine2Detail",
                c => new
                    {
                        TP2DetailId = c.Long(nullable: false, identity: true),
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
                        TP2InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TP2DetailId)
                .ForeignKey("dbo.tblTransferStockToPackagingLine2Info", t => t.TP2InfoId, cascadeDelete: true)
                .Index(t => t.TP2InfoId);
            
            CreateTable(
                "dbo.tblTransferStockToPackagingLine2Info",
                c => new
                    {
                        TP2InfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        PackagingLineFromId = c.Long(),
                        PackagingLineToId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ForQty = c.Int(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TP2InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferStockToPackagingLine2Detail", "TP2InfoId", "dbo.tblTransferStockToPackagingLine2Info");
            DropIndex("dbo.tblTransferStockToPackagingLine2Detail", new[] { "TP2InfoId" });
            DropTable("dbo.tblTransferStockToPackagingLine2Info");
            DropTable("dbo.tblTransferStockToPackagingLine2Detail");
        }
    }
}
