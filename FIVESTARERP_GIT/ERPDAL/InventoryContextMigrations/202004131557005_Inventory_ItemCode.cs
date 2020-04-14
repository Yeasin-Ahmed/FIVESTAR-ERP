namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_ItemCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItems", "ItemCode", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItems", "ItemCode");
        }
    }
}
