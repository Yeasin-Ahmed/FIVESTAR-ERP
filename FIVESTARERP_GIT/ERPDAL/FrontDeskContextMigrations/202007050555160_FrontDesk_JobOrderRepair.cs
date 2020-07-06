namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderRepair : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblJobOrderRepair",
                c => new
                    {
                        JobOrderRepairId = c.Long(nullable: false, identity: true),
                        RepairId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        JobOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderRepairId)
                .ForeignKey("dbo.tblJobOrders", t => t.JobOrderId, cascadeDelete: true)
                .Index(t => t.JobOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblJobOrderRepair", "JobOrderId", "dbo.tblJobOrders");
            DropIndex("dbo.tblJobOrderRepair", new[] { "JobOrderId" });
            DropTable("dbo.tblJobOrderRepair");
        }
    }
}
