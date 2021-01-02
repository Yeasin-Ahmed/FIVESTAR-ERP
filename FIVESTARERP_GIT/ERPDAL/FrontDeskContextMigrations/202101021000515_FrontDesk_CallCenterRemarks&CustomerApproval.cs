namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_CallCenterRemarksCustomerApproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CustomerApproval", c => c.String());
            AddColumn("dbo.tblJobOrders", "CallCenterRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CallCenterRemarks");
            DropColumn("dbo.tblJobOrders", "CustomerApproval");
        }
    }
}
