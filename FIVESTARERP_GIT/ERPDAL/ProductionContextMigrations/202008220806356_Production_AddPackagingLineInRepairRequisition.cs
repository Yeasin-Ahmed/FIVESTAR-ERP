namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingLineInRepairRequisition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "ReqFor", c => c.String(maxLength: 50));
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "PackagingLineName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "ReqFor", c => c.String(maxLength: 50));
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "PackagingLineName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "PackagingLineName");
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "PackagingLineId");
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "ReqFor");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "PackagingLineName");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "PackagingLineId");
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "ReqFor");
        }
    }
}
