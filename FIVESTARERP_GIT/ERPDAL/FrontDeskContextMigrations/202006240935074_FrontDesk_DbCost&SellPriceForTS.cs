namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbCostSellPriceForTS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTechnicalServicesStock", "CostPrice", c => c.Double(nullable: false));
            AddColumn("dbo.tblTechnicalServicesStock", "SellPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServicesStock", "SellPrice");
            DropColumn("dbo.tblTechnicalServicesStock", "CostPrice");
        }
    }
}
