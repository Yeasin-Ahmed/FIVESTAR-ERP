namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingLineNameAndQCInQRCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "QCLineId", c => c.Long());
            AddColumn("dbo.tblQRCodeTrace", "QCLineName", c => c.String());
            AddColumn("dbo.tblQRCodeTrace", "PackagingLineName", c => c.String());
            AddColumn("dbo.tblTempQRCodeTrace", "QCLineId", c => c.Long());
            AddColumn("dbo.tblTempQRCodeTrace", "QCLineName", c => c.String());
            AddColumn("dbo.tblTempQRCodeTrace", "PackagingLineName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTempQRCodeTrace", "PackagingLineName");
            DropColumn("dbo.tblTempQRCodeTrace", "QCLineName");
            DropColumn("dbo.tblTempQRCodeTrace", "QCLineId");
            DropColumn("dbo.tblQRCodeTrace", "PackagingLineName");
            DropColumn("dbo.tblQRCodeTrace", "QCLineName");
            DropColumn("dbo.tblQRCodeTrace", "QCLineId");
        }
    }
}
