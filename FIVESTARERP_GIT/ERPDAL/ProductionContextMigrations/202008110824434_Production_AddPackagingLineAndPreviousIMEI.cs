namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingLineAndPreviousIMEI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblQRCodeTrace", "PreviousIMEI", c => c.String(maxLength: 300));
            AddColumn("dbo.tblTempQRCodeTrace", "PackagingLineId", c => c.Long());
            AddColumn("dbo.tblTempQRCodeTrace", "PreviousIMEI", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTempQRCodeTrace", "PreviousIMEI");
            DropColumn("dbo.tblTempQRCodeTrace", "PackagingLineId");
            DropColumn("dbo.tblQRCodeTrace", "PreviousIMEI");
            DropColumn("dbo.tblQRCodeTrace", "PackagingLineId");
        }
    }
}
