namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddFloorNameInRepairItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRepairItem", "ProductionFloorName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRepairItem", "ProductionFloorName");
        }
    }
}
