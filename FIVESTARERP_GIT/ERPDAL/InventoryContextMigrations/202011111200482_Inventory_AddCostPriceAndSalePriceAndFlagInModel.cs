namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddCostPriceAndSalePriceAndFlagInModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDescriptions", "Flag", c => c.String());
            AddColumn("dbo.tblDescriptions", "CostPrice", c => c.Double(nullable: false));
            AddColumn("dbo.tblDescriptions", "SalePrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDescriptions", "SalePrice");
            DropColumn("dbo.tblDescriptions", "CostPrice");
            DropColumn("dbo.tblDescriptions", "Flag");
        }
    }
}
