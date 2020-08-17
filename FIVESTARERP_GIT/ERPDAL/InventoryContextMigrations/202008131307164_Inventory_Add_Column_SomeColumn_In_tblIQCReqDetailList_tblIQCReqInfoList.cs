namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_Add_Column_SomeColumn_In_tblIQCReqDetailList_tblIQCReqInfoList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIQCItemReqDetailList", "WellGoodsQty", c => c.Int());
            AddColumn("dbo.tblIQCItemReqDetailList", "FaultyGoodsQty", c => c.Int());
            AddColumn("dbo.tblIQCItemReqInfoList", "ReturnUserId", c => c.Long());
            AddColumn("dbo.tblIQCItemReqInfoList", "ReturnUserDate", c => c.DateTime());
            AddColumn("dbo.tblIQCItemReqInfoList", "ReturnReaciveUserId", c => c.Long());
            AddColumn("dbo.tblIQCItemReqInfoList", "ReturnReaciveUserDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblIQCItemReqInfoList", "ReturnReaciveUserDate");
            DropColumn("dbo.tblIQCItemReqInfoList", "ReturnReaciveUserId");
            DropColumn("dbo.tblIQCItemReqInfoList", "ReturnUserDate");
            DropColumn("dbo.tblIQCItemReqInfoList", "ReturnUserId");
            DropColumn("dbo.tblIQCItemReqDetailList", "FaultyGoodsQty");
            DropColumn("dbo.tblIQCItemReqDetailList", "WellGoodsQty");
        }
    }
}
