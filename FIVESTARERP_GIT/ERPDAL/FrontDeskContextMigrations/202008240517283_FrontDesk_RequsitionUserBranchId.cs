namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_RequsitionUserBranchId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionDetailForJobOrders", "UserBranchId", c => c.Long());
            AddColumn("dbo.tblRequsitionInfoForJobOrders", "UserBranchId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfoForJobOrders", "UserBranchId");
            DropColumn("dbo.tblRequsitionDetailForJobOrders", "UserBranchId");
        }
    }
}
