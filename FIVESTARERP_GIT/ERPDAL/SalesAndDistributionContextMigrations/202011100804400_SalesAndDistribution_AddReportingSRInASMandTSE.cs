namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddReportingSRInASMandTSE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblASM", "RSMId", c => c.Long(nullable: false));
            AddColumn("dbo.tblTSE", "ASMId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTSE", "ASMId");
            DropColumn("dbo.tblASM", "RSMId");
        }
    }
}
