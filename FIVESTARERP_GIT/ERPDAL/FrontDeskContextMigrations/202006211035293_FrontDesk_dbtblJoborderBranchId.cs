namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_dbtblJoborderBranchId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "BranchId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "BranchId");
        }
    }
}
