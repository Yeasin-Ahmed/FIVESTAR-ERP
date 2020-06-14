namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_SupplierAndStockInOrderQty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSupplier",
                c => new
                    {
                        SupplierId = c.Long(nullable: false, identity: true),
                        SupplierName = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            AddColumn("dbo.tblWarehouseStockDetails", "OrderQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblWarehouseStockDetails", "SupplierId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblWarehouseStockDetails", "SupplierId");
            DropColumn("dbo.tblWarehouseStockDetails", "OrderQty");
            DropTable("dbo.tblSupplier");
        }
    }
}
