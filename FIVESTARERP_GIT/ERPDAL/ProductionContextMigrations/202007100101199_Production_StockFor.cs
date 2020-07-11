namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_StockFor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblProductionStockDetail", "StockFor", c => c.String(maxLength: 100));
            AddColumn("dbo.tblProductionStockInfo", "StockFor", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblProductionStockInfo", "StockFor");
            DropColumn("dbo.tblProductionStockDetail", "StockFor");
        }
    }
}
