namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderMultipleCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "MultipleJobOrderCode", c => c.String());
            AddColumn("dbo.tblJobOrders", "JobSource", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "JobSource");
            DropColumn("dbo.tblJobOrders", "MultipleJobOrderCode");
        }
    }
}
