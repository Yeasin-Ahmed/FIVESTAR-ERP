namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_AddIQCReqCodeProperty_NonNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblIQCItemReqInfoList", "IQCReqCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblIQCItemReqInfoList", "IQCReqCode", c => c.DateTime());
        }
    }
}
