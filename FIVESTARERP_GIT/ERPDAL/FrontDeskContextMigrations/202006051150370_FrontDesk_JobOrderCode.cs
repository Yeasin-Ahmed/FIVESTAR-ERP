namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "JobOrderCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "JobOrderCode");
        }
    }
}
