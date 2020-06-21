namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_UpdateTransferToAssemly : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferStockToAssemblyDetail", "WarehouseId", c => c.Long());
            AddColumn("dbo.tblTransferStockToAssemblyInfo", "ItemTypeId", c => c.Long());
            AddColumn("dbo.tblTransferStockToAssemblyInfo", "ItemId", c => c.Long());
            AddColumn("dbo.tblTransferStockToAssemblyInfo", "ForQty", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferStockToAssemblyInfo", "ForQty");
            DropColumn("dbo.tblTransferStockToAssemblyInfo", "ItemId");
            DropColumn("dbo.tblTransferStockToAssemblyInfo", "ItemTypeId");
            DropColumn("dbo.tblTransferStockToAssemblyDetail", "WarehouseId");
        }
    }
}
