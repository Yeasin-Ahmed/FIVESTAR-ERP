namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderCustomerSupportTypeChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblJobOrders", "CSModel", c => c.Long());
            AlterColumn("dbo.tblJobOrders", "CSColor", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblJobOrders", "CSColor", c => c.String());
            AlterColumn("dbo.tblJobOrders", "CSModel", c => c.String());
        }
    }
}
