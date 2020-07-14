namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_jobOrderTsRepairStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "TsRepairStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "TsRepairStatus");
        }
    }
}
