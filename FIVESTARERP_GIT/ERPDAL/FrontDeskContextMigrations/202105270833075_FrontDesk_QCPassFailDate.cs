namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_QCPassFailDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "QCPassFailDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "QCPassFailDate");
        }
    }
}
