namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TSStockModelId_ : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblTechnicalServicesStock", "ModelId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblTechnicalServicesStock", "ModelId", c => c.String());
        }
    }
}
