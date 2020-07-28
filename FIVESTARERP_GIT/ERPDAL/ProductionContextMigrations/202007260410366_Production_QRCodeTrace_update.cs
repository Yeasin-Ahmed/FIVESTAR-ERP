namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QRCodeTrace_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "IMEI", c => c.String(maxLength: 300));
            AddColumn("dbo.tblQRCodeTrace", "BatteryCode", c => c.String(maxLength: 200));
            AddColumn("dbo.tblQRCodeTrace", "StateStatus", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQRCodeTrace", "StateStatus");
            DropColumn("dbo.tblQRCodeTrace", "BatteryCode");
            DropColumn("dbo.tblQRCodeTrace", "IMEI");
        }
    }
}
