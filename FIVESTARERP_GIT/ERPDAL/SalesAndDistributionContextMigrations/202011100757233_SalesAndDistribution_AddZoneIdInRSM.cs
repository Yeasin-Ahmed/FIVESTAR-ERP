namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddZoneIdInRSM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRSM", "ZoneId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRSM", "ZoneId");
        }
    }
}
