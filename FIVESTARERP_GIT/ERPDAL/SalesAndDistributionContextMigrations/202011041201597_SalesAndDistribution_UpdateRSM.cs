namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_UpdateRSM : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tblRSM", "UserName");
            DropColumn("dbo.tblRSM", "Password");
            DropColumn("dbo.tblRSM", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblRSM", "ConfirmPassword", c => c.String());
            AddColumn("dbo.tblRSM", "Password", c => c.String());
            AddColumn("dbo.tblRSM", "UserName", c => c.String());
        }
    }
}
