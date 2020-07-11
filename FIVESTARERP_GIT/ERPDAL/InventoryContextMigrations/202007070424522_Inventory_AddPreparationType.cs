namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddPreparationType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemPreparationInfo", "PreparationType", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemPreparationInfo", "PreparationType");
        }
    }
}
