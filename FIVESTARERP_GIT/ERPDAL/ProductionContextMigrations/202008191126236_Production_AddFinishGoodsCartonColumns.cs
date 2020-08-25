namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddFinishGoodsCartonColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "DescriptionId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "WarehouseId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "QRCode", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "IMEI", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "AllIMEI", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "CartoonNo", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "Width", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "Height", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "GrossWeight", c => c.String(maxLength: 150));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "NetWeight", c => c.String(maxLength: 150));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "TotalQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "TotalQty");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "NetWeight");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "GrossWeight");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "Height");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "Width");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "CartoonNo");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "AllIMEI");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "IMEI");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "QRCode");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "WarehouseId");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "DescriptionId");
        }
    }
}
