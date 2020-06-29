namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbCustomerTSBranchAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblCustomers", "BranchId", c => c.Long(nullable: false));
            AddColumn("dbo.tblTechnicalServiceEngs", "BranchId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServiceEngs", "BranchId");
            DropColumn("dbo.tblCustomers", "BranchId");
        }
    }
}
