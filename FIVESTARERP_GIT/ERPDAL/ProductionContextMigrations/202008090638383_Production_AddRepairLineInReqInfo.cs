namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddRepairLineInReqInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisitionItemInfo", "RepairLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisitionItemInfo", "RepairLineId");
        }
    }
}
