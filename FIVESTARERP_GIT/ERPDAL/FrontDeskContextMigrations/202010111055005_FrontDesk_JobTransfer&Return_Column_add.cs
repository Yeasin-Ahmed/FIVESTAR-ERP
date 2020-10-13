namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobTransferReturn_Column_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrderReturnDetails", "CourierName", c => c.String());
            AddColumn("dbo.tblJobOrderReturnDetails", "CourierNumber", c => c.String());
            AddColumn("dbo.tblJobOrderReturnDetails", "ApproxBill", c => c.String());
            AddColumn("dbo.tblJobOrderTransferDetail", "CourierName", c => c.String());
            AddColumn("dbo.tblJobOrderTransferDetail", "CourierNumber", c => c.String());
            AddColumn("dbo.tblJobOrderTransferDetail", "ApproxBill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrderTransferDetail", "ApproxBill");
            DropColumn("dbo.tblJobOrderTransferDetail", "CourierNumber");
            DropColumn("dbo.tblJobOrderTransferDetail", "CourierName");
            DropColumn("dbo.tblJobOrderReturnDetails", "ApproxBill");
            DropColumn("dbo.tblJobOrderReturnDetails", "CourierNumber");
            DropColumn("dbo.tblJobOrderReturnDetails", "CourierName");
        }
    }
}
