namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_UpdateSupplierTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblSupplier", "Email", c => c.String(maxLength: 150));
            AddColumn("dbo.tblSupplier", "Address", c => c.String(maxLength: 150));
            AddColumn("dbo.tblSupplier", "PhoneNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.tblSupplier", "MobileNumber", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblSupplier", "MobileNumber");
            DropColumn("dbo.tblSupplier", "PhoneNumber");
            DropColumn("dbo.tblSupplier", "Address");
            DropColumn("dbo.tblSupplier", "Email");
        }
    }
}
