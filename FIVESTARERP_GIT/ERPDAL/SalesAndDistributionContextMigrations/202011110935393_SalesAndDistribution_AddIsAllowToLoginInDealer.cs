namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddIsAllowToLoginInDealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDealer", "IsAllowToLogIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblDealer", "UserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDealer", "UserId");
            DropColumn("dbo.tblDealer", "IsAllowToLogIn");
        }
    }
}
