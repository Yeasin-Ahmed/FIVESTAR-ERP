namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddRSM_EUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRSM", "EUserId", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRSM", "EUserId");
        }
    }
}
