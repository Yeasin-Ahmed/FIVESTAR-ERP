namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_edit_dataType_tblInvoiceDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceDetails", "PartsId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceDetails", "PartsId", c => c.Long(nullable: false));
        }
    }
}
