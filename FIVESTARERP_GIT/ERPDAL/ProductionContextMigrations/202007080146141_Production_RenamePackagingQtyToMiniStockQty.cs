namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RenamePackagingQtyToMiniStockQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCItemStockDetail", "Flag", c => c.String(maxLength: 50));
            AddColumn("dbo.tblQCItemStockInfo", "MiniStockQty", c => c.Int(nullable: false));
            DropColumn("dbo.tblQCItemStockInfo", "PackagingQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblQCItemStockInfo", "PackagingQty", c => c.Int(nullable: false));
            DropColumn("dbo.tblQCItemStockInfo", "MiniStockQty");
            DropColumn("dbo.tblQCItemStockDetail", "Flag");
        }
    }
}
