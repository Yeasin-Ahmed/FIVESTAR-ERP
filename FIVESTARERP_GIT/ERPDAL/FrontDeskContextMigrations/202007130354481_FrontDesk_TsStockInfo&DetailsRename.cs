namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TsStockInfoDetailsRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTsStockReturnDetails", "ReqInfoId", c => c.Long(nullable: false));
            DropColumn("dbo.tblTsStockReturnDetails", "ReqDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblTsStockReturnDetails", "ReqDetailId", c => c.Long(nullable: false));
            DropColumn("dbo.tblTsStockReturnDetails", "ReqInfoId");
        }
    }
}
