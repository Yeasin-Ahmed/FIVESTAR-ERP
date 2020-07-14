namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QRCodeInFaultyCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyCase", "QRCode", c => c.String(maxLength: 100));
        }
        public override void Down()
        {
            DropColumn("dbo.tblFaultyCase", "QRCode");
        }
    }
}
