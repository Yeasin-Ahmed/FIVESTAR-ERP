namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QCPassCodeInQCPassInfoTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCPassTransferInformation", "QCPassCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQCPassTransferInformation", "QCPassCode");
        }
    }
}
