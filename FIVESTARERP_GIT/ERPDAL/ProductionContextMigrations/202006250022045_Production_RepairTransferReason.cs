namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairTransferReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferFromQCInfo", "RepairTransferReason", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferFromQCInfo", "RepairTransferReason");
        }
    }
}
