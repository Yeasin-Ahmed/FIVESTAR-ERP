namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_STransferModelToModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblStockTransferDetailsMToM",
                c => new
                    {
                        STransferDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        FromDescriptionId = c.Long(),
                        ToDescriptionId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockStatus = c.String(),
                        RefferenceNumber = c.String(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        STransferInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.STransferDetailId)
                .ForeignKey("dbo.tblStockTransferInfoMToM", t => t.STransferInfoId, cascadeDelete: true)
                .Index(t => t.STransferInfoId);
            
            CreateTable(
                "dbo.tblStockTransferInfoMToM",
                c => new
                    {
                        STransferInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        FromDescriptionId = c.Long(),
                        ToDescriptionId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.STransferInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblStockTransferDetailsMToM", "STransferInfoId", "dbo.tblStockTransferInfoMToM");
            DropIndex("dbo.tblStockTransferDetailsMToM", new[] { "STransferInfoId" });
            DropTable("dbo.tblStockTransferInfoMToM");
            DropTable("dbo.tblStockTransferDetailsMToM");
        }
    }
}
