namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddTransferIdAndTransferCodeInFaultyItemStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyItemStockDetail", "TransferId", c => c.Long());
            AddColumn("dbo.tblFaultyItemStockDetail", "TransferCode", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFaultyItemStockDetail", "TransferCode");
            DropColumn("dbo.tblFaultyItemStockDetail", "TransferId");
        }
    }
}
