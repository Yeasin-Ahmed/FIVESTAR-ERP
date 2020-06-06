namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbtblmobilePartStockInfoDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMobilePartStockDetails",
                c => new
                    {
                        MobilePartStockDetailId = c.Long(nullable: false, identity: true),
                        MobilePartId = c.Long(),
                        SWarehouseId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MobilePartStockDetailId);
            
            CreateTable(
                "dbo.tblMobilePartStockInfo",
                c => new
                    {
                        MobilePartStockInfoId = c.Long(nullable: false, identity: true),
                        MobilePartId = c.Long(),
                        SWarehouseId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MobilePartStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblMobilePartStockInfo");
            DropTable("dbo.tblMobilePartStockDetails");
        }
    }
}
