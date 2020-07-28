namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyLineInTransferFromQCInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferRepairItemToQcInfo", "AssemblyLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferRepairItemToQcInfo", "AssemblyLineId");
        }
    }
}
