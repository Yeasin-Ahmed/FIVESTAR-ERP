namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TSStockTSIdStatusAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTechnicalServicesStock", "TSId", c => c.Long(nullable: false));
            AddColumn("dbo.tblTechnicalServicesStock", "StateStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServicesStock", "StateStatus");
            DropColumn("dbo.tblTechnicalServicesStock", "TSId");
        }
    }
}
