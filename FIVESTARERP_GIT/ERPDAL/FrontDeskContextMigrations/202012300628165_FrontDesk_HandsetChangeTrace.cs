namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_HandsetChangeTrace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblHandsetChangeTraces",
                c => new
                    {
                        HandsetChangeTraceId = c.Long(nullable: false, identity: true),
                        JobOrderId = c.Long(nullable: false),
                        JobOrderCode = c.String(),
                        JobStatus = c.String(),
                        ModelId = c.Long(nullable: false),
                        Type = c.String(),
                        IMEI1 = c.String(),
                        IMEI2 = c.String(),
                        Color = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HandsetChangeTraceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblHandsetChangeTraces");
        }
    }
}
