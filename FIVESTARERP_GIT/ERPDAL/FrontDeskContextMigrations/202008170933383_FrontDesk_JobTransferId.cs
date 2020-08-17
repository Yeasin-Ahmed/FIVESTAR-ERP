namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobTransferId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "IsTransfer", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblJobOrders", "TransferBranchId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "TransferBranchId");
            DropColumn("dbo.tblJobOrders", "IsTransfer");
        }
    }
}
