namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_IQCForignKey_NonNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList");
            DropIndex("dbo.tblIQCItemReqDetailList", new[] { "IQCItemReqInfoId" });
            AlterColumn("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", c => c.Long(nullable: false));
            CreateIndex("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId");
            AddForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList", "IQCItemReqInfoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList");
            DropIndex("dbo.tblIQCItemReqDetailList", new[] { "IQCItemReqInfoId" });
            AlterColumn("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", c => c.Long());
            CreateIndex("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId");
            AddForeignKey("dbo.tblIQCItemReqDetailList", "IQCItemReqInfoId", "dbo.tblIQCItemReqInfoList", "IQCItemReqInfoId");
        }
    }
}
