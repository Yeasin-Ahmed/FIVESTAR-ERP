namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_MultipleJobDelivey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "MultipleDeliveryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "MultipleDeliveryCode");
        }
    }
}
