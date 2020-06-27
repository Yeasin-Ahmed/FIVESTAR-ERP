namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbJobOrderIMEIAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "IMEI", c => c.Int(nullable: false));
            AddColumn("dbo.tblJobOrders", "Type", c => c.String());
            AddColumn("dbo.tblJobOrders", "ModelColor", c => c.String());
            AddColumn("dbo.tblJobOrders", "WarrantyDate", c => c.DateTime());
            AddColumn("dbo.tblJobOrders", "Remarks", c => c.String());
            AddColumn("dbo.tblJobOrders", "ReferenceNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "ReferenceNumber");
            DropColumn("dbo.tblJobOrders", "Remarks");
            DropColumn("dbo.tblJobOrders", "WarrantyDate");
            DropColumn("dbo.tblJobOrders", "ModelColor");
            DropColumn("dbo.tblJobOrders", "Type");
            DropColumn("dbo.tblJobOrders", "IMEI");
        }
    }
}
