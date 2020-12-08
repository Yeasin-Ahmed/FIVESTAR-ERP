namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_tblStockTransferModelToModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockTransferDetailModelToModels",
                c => new
                    {
                        TransferDetailModelToModelId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        ToDescriptionId = c.Long(),
                        PartsId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TransferInfoModelToModelId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TransferDetailModelToModelId)
                .ForeignKey("dbo.StockTransferInfoModelToModels", t => t.TransferInfoModelToModelId, cascadeDelete: true)
                .Index(t => t.TransferInfoModelToModelId);
            
            CreateTable(
                "dbo.StockTransferInfoModelToModels",
                c => new
                    {
                        TransferInfoModelToModelId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        DescriptionId = c.Long(),
                        ToDescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ToWarehouseId = c.Long(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransferInfoModelToModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockTransferDetailModelToModels", "TransferInfoModelToModelId", "dbo.StockTransferInfoModelToModels");
            DropIndex("dbo.StockTransferDetailModelToModels", new[] { "TransferInfoModelToModelId" });
            DropTable("dbo.StockTransferInfoModelToModels");
            DropTable("dbo.StockTransferDetailModelToModels");
        }
    }
}
