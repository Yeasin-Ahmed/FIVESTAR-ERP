namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_IQCForignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", c => c.Long());
            CreateIndex("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId");
            AddForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList", "IQCItemReqInfoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList");
            DropIndex("dbo.tblIQCItemReqDetailList", new[] { "IQCItemReqInfoId" });
            DropColumn("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId");
        }
    }
}
