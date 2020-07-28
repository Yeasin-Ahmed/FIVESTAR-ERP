namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddUnitInReqItemDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisitionItemDetail", "UnitId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisitionItemDetail", "UnitId");
        }
    }
}
