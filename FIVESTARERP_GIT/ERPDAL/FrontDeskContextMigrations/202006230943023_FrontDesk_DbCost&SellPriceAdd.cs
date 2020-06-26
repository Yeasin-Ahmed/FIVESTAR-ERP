namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbCostSellPriceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionDetailForJobOrders", "CostPrice", c => c.Double(nullable: false));
            AddColumn("dbo.tblRequsitionDetailForJobOrders", "SellPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionDetailForJobOrders", "SellPrice");
            DropColumn("dbo.tblRequsitionDetailForJobOrders", "CostPrice");
        }
    }
}
