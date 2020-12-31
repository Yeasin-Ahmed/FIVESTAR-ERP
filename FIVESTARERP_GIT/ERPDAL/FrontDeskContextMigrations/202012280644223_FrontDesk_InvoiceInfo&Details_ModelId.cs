namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_InvoiceInfoDetails_ModelId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceDetails", "ModelId", c => c.Long());
            AddColumn("dbo.InvoiceDetails", "ModelName", c => c.String());
            AddColumn("dbo.tblInvoiceInfo", "ModelId", c => c.Long());
            AddColumn("dbo.tblInvoiceInfo", "ModelName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblInvoiceInfo", "ModelName");
            DropColumn("dbo.tblInvoiceInfo", "ModelId");
            DropColumn("dbo.InvoiceDetails", "ModelName");
            DropColumn("dbo.InvoiceDetails", "ModelId");
        }
    }
}
