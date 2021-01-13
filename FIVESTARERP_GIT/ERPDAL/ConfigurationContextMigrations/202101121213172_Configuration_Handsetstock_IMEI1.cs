namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_Handsetstock_IMEI1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblHandSetStock", "IMEI1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblHandSetStock", "IMEI1");
        }
    }
}
