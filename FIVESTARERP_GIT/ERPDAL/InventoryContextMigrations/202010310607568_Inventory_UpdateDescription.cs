namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_UpdateDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDescriptions", "StartPoint", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDescriptions", "StartPoint");
        }
    }
}
