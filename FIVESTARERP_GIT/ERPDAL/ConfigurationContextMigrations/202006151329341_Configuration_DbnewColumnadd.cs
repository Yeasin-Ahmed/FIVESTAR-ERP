namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbnewColumnadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "BranchFrom", c => c.Long());
            AddColumn("dbo.tblTransferDetails", "BranchTo", c => c.Long());
            AddColumn("dbo.tblTransferInfo", "ABWarehouse", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferInfo", "ABWarehouse");
            DropColumn("dbo.tblTransferDetails", "BranchTo");
            DropColumn("dbo.tblMobilePartStockDetails", "BranchFrom");
        }
    }
}
