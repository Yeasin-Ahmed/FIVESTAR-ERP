namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPakagingRepairTransferCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferToPackagingRepairInfo", "TransferCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferToPackagingRepairInfo", "TransferCode");
        }
    }
}
