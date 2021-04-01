namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderCustomerSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CustomerSupportStatus", c => c.String());
            AddColumn("dbo.tblJobOrders", "CSIMEI1", c => c.String());
            AddColumn("dbo.tblJobOrders", "CSIMEI2", c => c.String());
            AddColumn("dbo.tblJobOrders", "CSModel", c => c.String());
            AddColumn("dbo.tblJobOrders", "CSColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CSColor");
            DropColumn("dbo.tblJobOrders", "CSModel");
            DropColumn("dbo.tblJobOrders", "CSIMEI2");
            DropColumn("dbo.tblJobOrders", "CSIMEI1");
            DropColumn("dbo.tblJobOrders", "CustomerSupportStatus");
        }
    }
}
