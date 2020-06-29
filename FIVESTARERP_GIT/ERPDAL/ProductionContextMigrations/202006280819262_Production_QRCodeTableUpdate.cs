namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QRCodeTableUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "ProductionFloorName", c => c.String());
            AddColumn("dbo.tblQRCodeTrace", "ModelName", c => c.String());
            AddColumn("dbo.tblQRCodeTrace", "WarehouseName", c => c.String());
            AddColumn("dbo.tblQRCodeTrace", "ItemTypeName", c => c.String());
            AddColumn("dbo.tblQRCodeTrace", "ItemName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQRCodeTrace", "ItemName");
            DropColumn("dbo.tblQRCodeTrace", "ItemTypeName");
            DropColumn("dbo.tblQRCodeTrace", "WarehouseName");
            DropColumn("dbo.tblQRCodeTrace", "ModelName");
            DropColumn("dbo.tblQRCodeTrace", "ProductionFloorName");
        }
    }
}
