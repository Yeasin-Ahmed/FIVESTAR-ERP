namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbIMEI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblJobOrders", "IMEI", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblJobOrders", "IMEI", c => c.Int(nullable: false));
        }
    }
}
