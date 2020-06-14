namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbBranch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "BranchId", c => c.Long(nullable: false));
            AddColumn("dbo.tblMobilePartStockInfo", "BranchId", c => c.Long(nullable: false));
            AddColumn("dbo.tblServiceWarehouses", "BranchId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblServiceWarehouses", "BranchId");
            DropColumn("dbo.tblMobilePartStockInfo", "BranchId");
            DropColumn("dbo.tblMobilePartStockDetails", "BranchId");
        }
    }
}
