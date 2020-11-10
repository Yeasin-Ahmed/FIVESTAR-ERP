namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddRepresentativeFlagInDealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDealer", "RepresentativeFlag", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDealer", "RepresentativeFlag");
        }
    }
}
