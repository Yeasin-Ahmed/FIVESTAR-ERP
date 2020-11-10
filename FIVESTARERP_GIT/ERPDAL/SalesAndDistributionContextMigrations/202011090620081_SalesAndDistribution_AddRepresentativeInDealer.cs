namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddRepresentativeInDealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDealer", "RepresentativeId", c => c.Long(nullable: false));
            AddColumn("dbo.tblDealer", "RepresentativeUserId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDealer", "RepresentativeUserId");
            DropColumn("dbo.tblDealer", "RepresentativeId");
        }
    }
}
