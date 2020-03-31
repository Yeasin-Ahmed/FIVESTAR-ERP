namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddDescriptionId_ProductionStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblProductionStockDetail", "DescriptionId", c => c.Long());
            AddColumn("dbo.tblProductionStockInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblProductionStockInfo", "DescriptionId");
            DropColumn("dbo.tblProductionStockDetail", "DescriptionId");
        }
    }
}
