namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobCloseDateCUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CloseDate", c => c.DateTime());
            AddColumn("dbo.tblJobOrders", "CUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CUserId");
            DropColumn("dbo.tblJobOrders", "CloseDate");
        }
    }
}
