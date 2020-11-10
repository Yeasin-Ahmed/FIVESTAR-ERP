namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_RenameColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblASM", "RSMUserId", c => c.Long(nullable: false));
            AddColumn("dbo.tblTSE", "ASMUserId", c => c.Long(nullable: false));
            DropColumn("dbo.tblASM", "RSMID");
            DropColumn("dbo.tblTSE", "ASMID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblTSE", "ASMID", c => c.Long(nullable: false));
            AddColumn("dbo.tblASM", "RSMID", c => c.Long(nullable: false));
            DropColumn("dbo.tblTSE", "ASMUserId");
            DropColumn("dbo.tblASM", "RSMUserId");
        }
    }
}
