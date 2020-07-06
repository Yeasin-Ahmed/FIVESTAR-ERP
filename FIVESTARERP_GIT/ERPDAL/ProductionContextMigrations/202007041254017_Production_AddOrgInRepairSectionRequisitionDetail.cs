namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddOrgInRepairSectionRequisitionDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairSectionRequisitionDetail", "OrganizationId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionRequisitionDetail", "OrganizationId");
        }
    }
}
