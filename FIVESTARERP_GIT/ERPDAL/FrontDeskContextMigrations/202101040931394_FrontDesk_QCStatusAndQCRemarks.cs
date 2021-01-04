namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_QCStatusAndQCRemarks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "QCStatus", c => c.String());
            AddColumn("dbo.tblJobOrders", "QCRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "QCRemarks");
            DropColumn("dbo.tblJobOrders", "QCStatus");
        }
    }
}
