namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_Column_InvoiceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblInvoiceInfo", "InvoiceType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblInvoiceInfo", "InvoiceType");
        }
    }
}
