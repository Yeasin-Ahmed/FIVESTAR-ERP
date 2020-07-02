namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairSectionTransfer_TableEdit : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tblRepairSectionFaultyItemRequisitionDetail", newName: "tblRepairSectionFaultyItemTransferDetail");
            RenameTable(name: "dbo.tblRepairSectionFaultyItemInfo", newName: "tblRepairSectionFaultyItemTransferInfo");
            AddColumn("dbo.tblRepairSectionFaultyItemTransferDetail", "UnitId", c => c.Long(nullable: false));
            AddColumn("dbo.tblRepairSectionFaultyItemTransferDetail", "UnitName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionFaultyItemTransferDetail", "UnitName");
            DropColumn("dbo.tblRepairSectionFaultyItemTransferDetail", "UnitId");
            RenameTable(name: "dbo.tblRepairSectionFaultyItemTransferInfo", newName: "tblRepairSectionFaultyItemInfo");
            RenameTable(name: "dbo.tblRepairSectionFaultyItemTransferDetail", newName: "tblRepairSectionFaultyItemRequisitionDetail");
        }
    }
}
