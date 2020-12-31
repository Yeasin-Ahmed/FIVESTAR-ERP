namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_ReturnStock_ModelId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTsStockReturnDetails", "ModelId", c => c.Long());
            AddColumn("dbo.tblTsStockReturnInfo", "ModelId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTsStockReturnInfo", "ModelId");
            DropColumn("dbo.tblTsStockReturnDetails", "ModelId");
        }
    }
}
