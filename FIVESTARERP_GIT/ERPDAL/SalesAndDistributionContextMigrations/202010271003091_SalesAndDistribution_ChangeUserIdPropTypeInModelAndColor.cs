namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_ChangeUserIdPropTypeInModelAndColor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblColor", "EUserId", c => c.Long());
            AlterColumn("dbo.tblColor", "UpUserId", c => c.Long());
            AlterColumn("dbo.tblModel", "EUserId", c => c.Long());
            AlterColumn("dbo.tblModel", "UpUserId", c => c.Long());
        }
        public override void Down()
        {
            AlterColumn("dbo.tblModel", "UpUserId", c => c.Boolean());
            AlterColumn("dbo.tblModel", "EUserId", c => c.Boolean());
            AlterColumn("dbo.tblColor", "UpUserId", c => c.Boolean());
            AlterColumn("dbo.tblColor", "EUserId", c => c.Boolean());
        }
    }
}
