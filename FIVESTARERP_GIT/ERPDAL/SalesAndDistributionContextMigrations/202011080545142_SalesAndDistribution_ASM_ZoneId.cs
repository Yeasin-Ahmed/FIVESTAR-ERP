namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_ASM_ZoneId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblASM", "ZoneId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblASM", "ZoneId");
        }
    }
}
