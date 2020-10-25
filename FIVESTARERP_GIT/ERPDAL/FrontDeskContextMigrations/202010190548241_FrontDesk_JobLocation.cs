namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "JobLocation", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "JobLocation");
        }
    }
}
