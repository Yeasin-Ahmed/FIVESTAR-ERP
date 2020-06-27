namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ReqUnitId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionInfo", "UnitId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfo", "UnitId");
        }
    }
}
