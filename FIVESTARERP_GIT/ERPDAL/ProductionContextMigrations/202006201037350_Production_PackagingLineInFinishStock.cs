namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_PackagingLineInFinishStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsInfo", "PackagingLineId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "PackagingLineId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsStockDetail", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblFinishGoodsStockInfo", "PackagingLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsStockInfo", "PackagingLineId");
            DropColumn("dbo.tblFinishGoodsStockDetail", "PackagingLineId");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "PackagingLineId");
            DropColumn("dbo.tblFinishGoodsInfo", "PackagingLineId");
        }
    }
}
