namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddTACInDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDescriptions", "TAC", c => c.String(maxLength: 10));
            AddColumn("dbo.tblDescriptions", "EndPoint", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDescriptions", "EndPoint");
            DropColumn("dbo.tblDescriptions", "TAC");
        }
    }
}
