namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_AddNColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMobileParts", "MobilePartCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMobileParts", "MobilePartCode");
        }
    }
}
