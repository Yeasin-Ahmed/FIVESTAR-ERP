namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_CourierAndApproxBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CourierNumber", c => c.String());
            AddColumn("dbo.tblJobOrders", "CourierName", c => c.String());
            AddColumn("dbo.tblJobOrders", "ApproxBill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "ApproxBill");
            DropColumn("dbo.tblJobOrders", "CourierName");
            DropColumn("dbo.tblJobOrders", "CourierNumber");
        }
    }
}
