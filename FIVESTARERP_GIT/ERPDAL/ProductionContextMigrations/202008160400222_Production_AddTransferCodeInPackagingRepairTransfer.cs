namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddTransferCodeInPackagingRepairTransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblIMEITransferToRepairDetail", "ProblemId", c => c.Long(nullable: false));
            AddColumn("dbo.tblIMEITransferToRepairDetail", "ProblemName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblTransferToPackagingRepairDetail", "TransferCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferToPackagingRepairDetail", "TransferCode");
            DropColumn("dbo.tblIMEITransferToRepairDetail", "ProblemName");
            DropColumn("dbo.tblIMEITransferToRepairDetail", "ProblemId");
        }
    }
}
