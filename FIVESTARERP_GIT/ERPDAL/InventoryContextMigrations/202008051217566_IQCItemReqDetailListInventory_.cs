namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IQCItemReqDetailListInventory_ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIQCItemReqInfoList", "SupplierId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblIQCItemReqInfoList", "SupplierId");
        }
    }
}
