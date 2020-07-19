namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddQRCode_AddIncomingTransferId_InTransferFromRepair : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferRepairItemToQcDetail", "QRCode", c => c.String(maxLength: 100));
            AddColumn("dbo.tblTransferRepairItemToQcDetail", "IncomingTransferId", c => c.Long(nullable: false));
            AddColumn("dbo.tblTransferRepairItemToQcDetail", "IncomingTransferCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferRepairItemToQcDetail", "IncomingTransferCode");
            DropColumn("dbo.tblTransferRepairItemToQcDetail", "IncomingTransferId");
            DropColumn("dbo.tblTransferRepairItemToQcDetail", "QRCode");
        }
    }
}
