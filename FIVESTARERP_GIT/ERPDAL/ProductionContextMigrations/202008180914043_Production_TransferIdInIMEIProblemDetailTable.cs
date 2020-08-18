namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferIdInIMEIProblemDetailTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIMEITransferToRepairDetail", "TransferId", c => c.Long(nullable: false));
            AddColumn("dbo.tblIMEITransferToRepairDetail", "TransferCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblIMEITransferToRepairDetail", "TransferCode");
            DropColumn("dbo.tblIMEITransferToRepairDetail", "TransferId");
        }
    }
}
