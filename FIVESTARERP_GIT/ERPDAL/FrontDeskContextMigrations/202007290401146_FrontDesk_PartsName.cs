namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_PartsName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceDetails", "PartsName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceDetails", "PartsName", c => c.Long(nullable: false));
        }
    }
}
