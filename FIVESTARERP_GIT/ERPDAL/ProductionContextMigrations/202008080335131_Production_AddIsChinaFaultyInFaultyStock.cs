namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddIsChinaFaultyInFaultyStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyItemStockDetail", "IsChinaFaulty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "IsChinaFaulty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblProductionFaultyStockDetail", "IsChinaFaulty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblProductionFaultyStockInfo", "IsChinaFaulty", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblProductionFaultyStockInfo", "IsChinaFaulty");
            DropColumn("dbo.tblProductionFaultyStockDetail", "IsChinaFaulty");
            DropColumn("dbo.tblFaultyItemStockInfo", "IsChinaFaulty");
            DropColumn("dbo.tblFaultyItemStockDetail", "IsChinaFaulty");
        }
    }
}
