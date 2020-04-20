namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FinishGoodsSend_RemarksUnitId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "UnitId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "RefferenceNumber", c => c.String(maxLength: 100));
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "RefferenceNumber", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "RefferenceNumber");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "RefferenceNumber");
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "UnitId");
        }
    }
}
