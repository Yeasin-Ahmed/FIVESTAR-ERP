namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_ServicesStockReqIdAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTechnicalServicesStock", "RequsitionInfoForJobOrderId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTechnicalServicesStock", "RequsitionInfoForJobOrderId");
        }
    }
}
