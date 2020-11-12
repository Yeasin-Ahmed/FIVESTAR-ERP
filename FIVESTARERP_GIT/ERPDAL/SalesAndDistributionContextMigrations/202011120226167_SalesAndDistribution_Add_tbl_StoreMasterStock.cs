namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_Add_tbl_StoreMasterStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblStoreMasterStock",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(),
                        BrandId = c.Long(),
                        Model = c.Long(),
                        ColorId = c.Long(),
                        IMEI = c.String(maxLength: 100),
                        CostPrice = c.Double(),
                        SalePrice = c.Double(),
                        StockInQty = c.Long(nullable: false),
                        StockOutQty = c.Long(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        LastStockInTime = c.DateTime(),
                        LastStockOutTime = c.DateTime(),
                        StockTransactionReason = c.String(maxLength: 100),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblStoreMasterStock");
        }
    }
}
