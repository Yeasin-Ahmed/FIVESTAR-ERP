namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_ItemStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblItemStock",
                c => new
                    {
                        StockId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ModelId = c.Long(nullable: false),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ColorId = c.Long(),
                        IMEI = c.String(maxLength: 100),
                        AllIMEI = c.String(maxLength: 100),
                        StockStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblItemStock");
        }
    }
}
