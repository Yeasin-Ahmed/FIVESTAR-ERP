namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QCItemProductionFloorId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCItemStockDetail", "ProductionFloorId", c => c.Long());
            AddColumn("dbo.tblQCItemStockInfo", "ProductionFloorId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQCItemStockInfo", "ProductionFloorId");
            DropColumn("dbo.tblQCItemStockDetail", "ProductionFloorId");
        }
    }
}
