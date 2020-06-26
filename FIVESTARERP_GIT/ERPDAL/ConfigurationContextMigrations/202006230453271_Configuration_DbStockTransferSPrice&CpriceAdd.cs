namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbStockTransferSPriceCpriceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferDetails", "SellPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferDetails", "SellPrice");
        }
    }
}
