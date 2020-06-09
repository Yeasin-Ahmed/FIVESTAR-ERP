namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbAddTSTableColum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTechnicalServiceEngs", "TsCode", c => c.String());
            AddColumn("dbo.tblTechnicalServiceEngs", "UserName", c => c.String());
            AddColumn("dbo.tblTechnicalServiceEngs", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServiceEngs", "Password");
            DropColumn("dbo.tblTechnicalServiceEngs", "UserName");
            DropColumn("dbo.tblTechnicalServiceEngs", "TsCode");
        }
    }
}
