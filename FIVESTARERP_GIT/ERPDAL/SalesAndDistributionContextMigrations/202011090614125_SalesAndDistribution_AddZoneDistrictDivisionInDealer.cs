namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddZoneDistrictDivisionInDealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDealer", "ZoneId", c => c.Long(nullable: false));
            AddColumn("dbo.tblDealer", "DistrictId", c => c.Long(nullable: false));
            AddColumn("dbo.tblDealer", "DivisionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDealer", "DivisionId");
            DropColumn("dbo.tblDealer", "DistrictId");
            DropColumn("dbo.tblDealer", "ZoneId");
        }
    }
}
