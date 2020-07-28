namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddAssemblyLineAndQRCodeInQCPass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCPassTransferInformation", "AssemblyLineId", c => c.Long(nullable: false));
            AddColumn("dbo.tblQCPassTransferInformation", "AssemblyLineName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblQCPassTransferDetail", "AssemblyLineId", c => c.Long(nullable: false));
            AddColumn("dbo.tblQCPassTransferDetail", "AssemblyLineName", c => c.String(maxLength: 100));
            AddColumn("dbo.tblQCPassTransferDetail", "QRCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQCPassTransferDetail", "QRCode");
            DropColumn("dbo.tblQCPassTransferDetail", "AssemblyLineName");
            DropColumn("dbo.tblQCPassTransferDetail", "AssemblyLineId");
            DropColumn("dbo.tblQCPassTransferInformation", "AssemblyLineName");
            DropColumn("dbo.tblQCPassTransferInformation", "AssemblyLineId");
        }
    }
}
