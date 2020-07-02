namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_UpdateRepairSectionRequisition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "RequisitionCode", c => c.String(maxLength: 100));
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "WarehouseId", c => c.Long());
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "WarehouseName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "RequisitionCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "RequisitionCode");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "WarehouseName");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "WarehouseId");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "RequisitionCode");
        }
    }
}
