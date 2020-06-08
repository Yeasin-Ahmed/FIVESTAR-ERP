namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_AddTSToJobOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "TSId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "TSId");
        }
    }
}
