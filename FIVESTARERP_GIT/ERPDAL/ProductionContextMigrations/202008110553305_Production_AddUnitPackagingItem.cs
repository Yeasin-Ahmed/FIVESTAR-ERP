namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddUnitPackagingItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblPackagingItemStockDetail", "UnitId", c => c.Long());
            AddColumn("dbo.tblPackagignItemStockInfo", "UnitId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblPackagignItemStockInfo", "UnitId");
            DropColumn("dbo.tblPackagingItemStockDetail", "UnitId");
        }
    }
}
