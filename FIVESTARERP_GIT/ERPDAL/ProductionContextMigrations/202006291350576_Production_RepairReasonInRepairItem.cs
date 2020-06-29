namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairReasonInRepairItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairItem", "RepairReason", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairItem", "RepairReason");
        }
    }
}
