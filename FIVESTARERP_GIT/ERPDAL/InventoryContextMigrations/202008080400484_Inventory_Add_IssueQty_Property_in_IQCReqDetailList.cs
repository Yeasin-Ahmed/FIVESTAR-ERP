namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_Add_IssueQty_Property_in_IQCReqDetailList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIQCItemReqDetailList", "IssueQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblIQCItemReqDetailList", "IssueQty");
        }
    }
}
