namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddCartonNoInFinishGoodsPackaging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "CartonNo", c => c.String(maxLength: 100));
            AddColumn("dbo.tblTempQRCodeTrace", "CartonNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTempQRCodeTrace", "CartonNo");
            DropColumn("dbo.tblQRCodeTrace", "CartonNo");
        }
    }
}
