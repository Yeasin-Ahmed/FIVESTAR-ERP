namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrder_IsReturn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "IsReturn", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "IsReturn");
        }
    }
}
