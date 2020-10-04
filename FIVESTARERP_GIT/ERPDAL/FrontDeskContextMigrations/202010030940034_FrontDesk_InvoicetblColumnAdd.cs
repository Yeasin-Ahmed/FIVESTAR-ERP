namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_InvoicetblColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblInvoiceInfo", "Email", c => c.String());
            AddColumn("dbo.tblInvoiceInfo", "Address", c => c.String());
            AddColumn("dbo.tblInvoiceInfo", "WarrentyFor", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblInvoiceInfo", "WarrentyFor");
            DropColumn("dbo.tblInvoiceInfo", "Address");
            DropColumn("dbo.tblInvoiceInfo", "Email");
        }
    }
}
