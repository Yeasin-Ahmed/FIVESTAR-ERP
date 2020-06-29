namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbIMEI2AndEndDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "IMEI2", c => c.String());
            AddColumn("dbo.tblJobOrders", "WarrantyEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "WarrantyEndDate");
            DropColumn("dbo.tblJobOrders", "IMEI2");
        }
    }
}
