namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_AddNewColumnsInJobOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "JobOrderType", c => c.String(maxLength: 50));
            AddColumn("dbo.tblJobOrders", "CustomerId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CustomerId");
            DropColumn("dbo.tblJobOrders", "JobOrderType");
        }
    }
}
