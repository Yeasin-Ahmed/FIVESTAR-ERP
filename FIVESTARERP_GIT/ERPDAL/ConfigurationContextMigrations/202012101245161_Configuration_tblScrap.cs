namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_tblScrap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblScrapStockDetail",
                c => new
                    {
                        ScrapStockDetailId = c.Long(nullable: false, identity: true),
                        FaultyStockAssignTSId = c.Long(nullable: false),
                        FaultyStockInfoId = c.Long(),
                        DescriptionId = c.Long(),
                        PartsId = c.Long(),
                        TSId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        RepairedQuantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ScrapStockInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ScrapStockDetailId);
            
            CreateTable(
                "dbo.tblScrapStockInfo",
                c => new
                    {
                        ScrapStockInfoId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        PartsId = c.Long(),
                        ScrapQuantity = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ScrapStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblScrapStockInfo");
            DropTable("dbo.tblScrapStockDetail");
        }
    }
}
