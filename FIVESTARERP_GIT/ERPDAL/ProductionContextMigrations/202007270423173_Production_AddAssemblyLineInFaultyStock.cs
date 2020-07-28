namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyLineInFaultyStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyItemStockDetail", "AsseemblyLineId", c => c.Long());
            AddColumn("dbo.tblFaultyItemStockInfo", "AsseemblyLineId", c => c.Long());
            AddColumn("dbo.tblRepairLineStockDetail", "AssemblyLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairLineStockDetail", "AssemblyLineId");
            DropColumn("dbo.tblFaultyItemStockInfo", "AsseemblyLineId");
            DropColumn("dbo.tblFaultyItemStockDetail", "AsseemblyLineId");
        }
    }
}
