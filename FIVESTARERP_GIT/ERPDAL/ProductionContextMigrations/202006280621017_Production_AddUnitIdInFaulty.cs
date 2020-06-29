namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddUnitIdInFaulty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyItemStockDetail", "UnitId", c => c.Long());
            AddColumn("dbo.tblFaultyItemStockInfo", "UnitId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFaultyItemStockInfo", "UnitId");
            DropColumn("dbo.tblFaultyItemStockDetail", "UnitId");
        }
    }
}
