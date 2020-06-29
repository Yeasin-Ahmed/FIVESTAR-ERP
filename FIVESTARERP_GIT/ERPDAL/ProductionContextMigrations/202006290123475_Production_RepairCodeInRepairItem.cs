namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairCodeInRepairItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairItem", "RepairCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairItem", "RepairCode");
        }
    }
}
