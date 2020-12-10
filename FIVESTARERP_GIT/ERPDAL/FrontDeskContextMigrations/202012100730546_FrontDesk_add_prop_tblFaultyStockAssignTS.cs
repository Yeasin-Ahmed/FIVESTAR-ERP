namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_add_prop_tblFaultyStockAssignTS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyStockAssignTS", "StateStatus", c => c.String());
            AddColumn("dbo.tblFaultyStockAssignTS", "RepairedQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.tblFaultyStockAssignTS", "ScrapQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFaultyStockAssignTS", "ScrapQuantity");
            DropColumn("dbo.tblFaultyStockAssignTS", "RepairedQuantity");
            DropColumn("dbo.tblFaultyStockAssignTS", "StateStatus");
        }
    }
}
