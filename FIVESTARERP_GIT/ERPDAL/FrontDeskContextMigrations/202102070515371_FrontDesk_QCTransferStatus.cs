namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_QCTransferStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "QCTransferStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "QCTransferStatus");
        }
    }
}
