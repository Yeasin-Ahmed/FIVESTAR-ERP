namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddFlagInModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDescriptions", "Flag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDescriptions", "Flag");
        }
    }
}
