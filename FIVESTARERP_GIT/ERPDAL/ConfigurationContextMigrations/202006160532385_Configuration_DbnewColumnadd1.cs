namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbnewColumnadd1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferInfo", "WarehouseIdTo", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferInfo", "WarehouseIdTo");
        }
    }
}
