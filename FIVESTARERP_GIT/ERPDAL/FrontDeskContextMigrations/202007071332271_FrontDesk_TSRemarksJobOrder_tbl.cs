namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TSRemarksJobOrder_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "TSRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "TSRemarks");
        }
    }
}
