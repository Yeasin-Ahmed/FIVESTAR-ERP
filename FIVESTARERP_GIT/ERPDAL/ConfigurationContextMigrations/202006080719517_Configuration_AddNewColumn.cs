namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_AddNewColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblAccessories", "AccessoriesCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblAccessories", "AccessoriesCode");
        }
    }
}
