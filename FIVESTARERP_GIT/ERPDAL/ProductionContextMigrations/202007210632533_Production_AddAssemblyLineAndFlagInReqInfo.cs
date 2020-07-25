namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyLineAndFlagInReqInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionInfo", "AssemblyLineId", c => c.Long());
            AddColumn("dbo.tblRequsitionInfo", "Flag", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfo", "Flag");
            DropColumn("dbo.tblRequsitionInfo", "AssemblyLineId");
        }
    }
}
