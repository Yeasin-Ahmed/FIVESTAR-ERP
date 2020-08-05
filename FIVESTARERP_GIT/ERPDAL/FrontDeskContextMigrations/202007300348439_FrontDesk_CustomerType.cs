namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_CustomerType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CustomerType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CustomerType");
        }
    }
}
