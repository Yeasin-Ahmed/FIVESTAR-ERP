namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobFaultJobServiceAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblJobOrderFault",
                c => new
                    {
                        JobOrderFaultId = c.Long(nullable: false, identity: true),
                        FaultId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        JobOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderFaultId)
                .ForeignKey("dbo.tblJobOrders", t => t.JobOrderId, cascadeDelete: true)
                .Index(t => t.JobOrderId);
            
            CreateTable(
                "dbo.tblJobOrderServices",
                c => new
                    {
                        JobOrderServiceId = c.Long(nullable: false, identity: true),
                        ServiceId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        JobOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderServiceId)
                .ForeignKey("dbo.tblJobOrders", t => t.JobOrderId, cascadeDelete: true)
                .Index(t => t.JobOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblJobOrderServices", "JobOrderId", "dbo.tblJobOrders");
            DropForeignKey("dbo.tblJobOrderFault", "JobOrderId", "dbo.tblJobOrders");
            DropIndex("dbo.tblJobOrderServices", new[] { "JobOrderId" });
            DropIndex("dbo.tblJobOrderFault", new[] { "JobOrderId" });
            DropTable("dbo.tblJobOrderServices");
            DropTable("dbo.tblJobOrderFault");
        }
    }
}
