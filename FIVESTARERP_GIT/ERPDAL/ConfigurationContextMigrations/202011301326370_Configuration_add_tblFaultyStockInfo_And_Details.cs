namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_tblFaultyStockInfo_And_Details : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFaultyStockDetails",
                c => new
                    {
                        FaultyStockDetailId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        JobOrderId = c.Long(),
                        SWarehouseId = c.Long(),
                        PartsId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        TSId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        FaultyStockInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FaultyStockDetailId);
            
            CreateTable(
                "dbo.tblFaultyStockInfo",
                c => new
                    {
                        FaultyStockInfoId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        JobOrderId = c.Long(),
                        SWarehouseId = c.Long(),
                        PartsId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        BranchId = c.Long(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FaultyStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFaultyStockInfo");
            DropTable("dbo.tblFaultyStockDetails");
        }
    }
}
