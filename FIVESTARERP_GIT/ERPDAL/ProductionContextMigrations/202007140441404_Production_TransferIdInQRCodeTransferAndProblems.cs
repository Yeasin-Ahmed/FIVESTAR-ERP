namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferIdInQRCodeTransferAndProblems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeProblem", "TransferId", c => c.Long(nullable: false));
            AddColumn("dbo.tblQRCodeProblem", "TransferCode", c => c.String(maxLength: 100));
            AddColumn("dbo.tblQRCodeTransferToRepairInfo", "TransferId", c => c.Long(nullable: false));
            AddColumn("dbo.tblQRCodeTransferToRepairInfo", "TransferCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQRCodeTransferToRepairInfo", "TransferCode");
            DropColumn("dbo.tblQRCodeTransferToRepairInfo", "TransferId");
            DropColumn("dbo.tblQRCodeProblem", "TransferCode");
            DropColumn("dbo.tblQRCodeProblem", "TransferId");
        }
    }
}
