namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyIdInQCAndTransferFromQC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCItemStockInfo", "AssemblyLineId", c => c.Long());
            AddColumn("dbo.tblTransferFromQCInfo", "AssemblyLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferFromQCInfo", "AssemblyLineId");
            DropColumn("dbo.tblQCItemStockInfo", "AssemblyLineId");
        }
    }
}
