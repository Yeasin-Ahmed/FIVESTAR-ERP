namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_UpdateItemStockTbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemStock", "CartoonNo", c => c.String(maxLength: 100));
            AddColumn("dbo.tblItemStock", "SaleDate", c => c.DateTime());
            AddColumn("dbo.tblItemStock", "ReferenceNumber", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemStock", "ReferenceNumber");
            DropColumn("dbo.tblItemStock", "SaleDate");
            DropColumn("dbo.tblItemStock", "CartoonNo");
        }
    }
}
