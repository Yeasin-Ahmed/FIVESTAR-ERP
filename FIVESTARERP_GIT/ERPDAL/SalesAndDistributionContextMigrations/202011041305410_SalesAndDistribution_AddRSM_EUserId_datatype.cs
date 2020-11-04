namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddRSM_EUserId_datatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblRSM", "EUserId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblRSM", "EUserId", c => c.DateTime());
        }
    }
}
