namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_UpdateTransferToQC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferStockToQCDetail", "WarehouseId", c => c.Long());
            AddColumn("dbo.tblTransferStockToQCInfo", "ItemTypeId", c => c.Long());
            AddColumn("dbo.tblTransferStockToQCInfo", "ItemId", c => c.Long());
            AddColumn("dbo.tblTransferStockToQCInfo", "ForQty", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferStockToQCInfo", "ForQty");
            DropColumn("dbo.tblTransferStockToQCInfo", "ItemId");
            DropColumn("dbo.tblTransferStockToQCInfo", "ItemTypeId");
            DropColumn("dbo.tblTransferStockToQCDetail", "WarehouseId");
        }
    }
}
