namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddHandOverInRepairRequisition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "HandOverId", c => c.Long());
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "HandOverDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "HandOverDate");
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "HandOverId");
        }
    }
}
