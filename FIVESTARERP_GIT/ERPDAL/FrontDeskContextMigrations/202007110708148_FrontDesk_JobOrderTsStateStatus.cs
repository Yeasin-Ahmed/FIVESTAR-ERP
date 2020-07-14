namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderTsStateStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrderTS", "JobOrderStateStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrderTS", "JobOrderStateStatus");
        }
    }
}
