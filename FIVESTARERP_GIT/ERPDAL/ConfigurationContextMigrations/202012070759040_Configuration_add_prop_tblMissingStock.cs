namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_prop_tblMissingStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMissingStock", "IMEI", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMissingStock", "IMEI");
        }
    }
}
