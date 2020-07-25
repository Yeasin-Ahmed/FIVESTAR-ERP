namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ProductionStock_TransferFor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferStockToAssemblyInfo", "RepairLineId", c => c.Long());
            AddColumn("dbo.tblTransferStockToAssemblyInfo", "TransferFor", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferStockToAssemblyInfo", "TransferFor");
            DropColumn("dbo.tblTransferStockToAssemblyInfo", "RepairLineId");
        }
    }
}
