namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_RefNumStockDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobilePartStockDetails", "ReferrenceNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMobilePartStockDetails", "ReferrenceNumber");
        }
    }
}
