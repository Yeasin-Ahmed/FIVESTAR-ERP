namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_UpdateTransferFromQC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferFromQCDetail", "WarehouseId", c => c.Long());
            AddColumn("dbo.tblTransferFromQCInfo", "ItemTypeId", c => c.Long());
            AddColumn("dbo.tblTransferFromQCInfo", "ItemId", c => c.Long());
            AddColumn("dbo.tblTransferFromQCInfo", "ForQty", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferFromQCInfo", "ForQty");
            DropColumn("dbo.tblTransferFromQCInfo", "ItemId");
            DropColumn("dbo.tblTransferFromQCInfo", "ItemTypeId");
            DropColumn("dbo.tblTransferFromQCDetail", "WarehouseId");
        }
    }
}
