namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobTransferBranchId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblJobOrders", "IsTransfer", c => c.Boolean());
            AlterColumn("dbo.tblJobOrders", "TransferBranchId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblJobOrders", "TransferBranchId", c => c.Long(nullable: false));
            AlterColumn("dbo.tblJobOrders", "IsTransfer", c => c.Boolean(nullable: false));
        }
    }
}
