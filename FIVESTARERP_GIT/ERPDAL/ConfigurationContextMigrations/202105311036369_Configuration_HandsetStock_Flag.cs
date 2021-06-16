namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_HandsetStock_Flag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblHandSetStock", "Flag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblHandSetStock", "Flag");
        }
    }
}
