namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_add_prop_tblInvoiceDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceDetails", "SalesType", c => c.String());
            AddColumn("dbo.InvoiceDetails", "IMEI", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceDetails", "IMEI");
            DropColumn("dbo.InvoiceDetails", "SalesType");
        }
    }
}
