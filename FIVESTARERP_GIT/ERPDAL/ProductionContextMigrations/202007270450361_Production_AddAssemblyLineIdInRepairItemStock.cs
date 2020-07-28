namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyLineIdInRepairItemStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairItemStockDetail", "AssemblyLineId", c => c.Long());
            AddColumn("dbo.tblRepairItemStockInfo", "AssemblyLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairItemStockInfo", "AssemblyLineId");
            DropColumn("dbo.tblRepairItemStockDetail", "AssemblyLineId");
        }
    }
}
