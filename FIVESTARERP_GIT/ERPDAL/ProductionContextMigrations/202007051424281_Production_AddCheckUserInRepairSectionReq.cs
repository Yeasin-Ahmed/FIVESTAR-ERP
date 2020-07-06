namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddCheckUserInRepairSectionReq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "CheckedBy", c => c.Long());
            AddColumn("dbo.tblRepairSectionRequisitionInfo", "CheckedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "CheckedDate");
            DropColumn("dbo.tblRepairSectionRequisitionInfo", "CheckedBy");
        }
    }
}
