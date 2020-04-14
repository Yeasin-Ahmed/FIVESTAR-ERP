namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FinishGoods_UnitId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsInfo", "UnitId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsRowMaterial", "UnitId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsRowMaterial", "UnitId");
            DropColumn("dbo.tblFinishGoodsInfo", "UnitId");
        }
    }
}
