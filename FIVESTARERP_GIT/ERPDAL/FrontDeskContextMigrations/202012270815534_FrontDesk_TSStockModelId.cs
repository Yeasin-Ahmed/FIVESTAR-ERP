namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TSStockModelId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTechnicalServicesStock", "ModelId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServicesStock", "ModelId");
        }
    }
}
