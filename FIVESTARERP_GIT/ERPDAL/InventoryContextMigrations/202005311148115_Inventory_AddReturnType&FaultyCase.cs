namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddReturnTypeFaultyCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairStockDetails", "ReturnType", c => c.String(maxLength: 50));
            AddColumn("dbo.tblRepairStockDetails", "FaultyCase", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairStockDetails", "FaultyCase");
            DropColumn("dbo.tblRepairStockDetails", "ReturnType");
        }
    }
}
