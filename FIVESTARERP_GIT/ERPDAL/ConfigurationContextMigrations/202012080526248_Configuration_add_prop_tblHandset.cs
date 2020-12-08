namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_prop_tblHandset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblHandSetStock", "StateStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblHandSetStock", "StateStatus");
        }
    }
}
