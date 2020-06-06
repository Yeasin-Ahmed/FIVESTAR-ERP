namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbInitialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblJobOrderAccessories",
                c => new
                    {
                        JobOrderAccessoriesId = c.Long(nullable: false, identity: true),
                        AccessoriesId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        JobOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderAccessoriesId)
                .ForeignKey("dbo.tblJobOrders", t => t.JobOrderId, cascadeDelete: true)
                .Index(t => t.JobOrderId);
            
            CreateTable(
                "dbo.tblJobOrders",
                c => new
                    {
                        JodOrderId = c.Long(nullable: false, identity: true),
                        CustomerName = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 15),
                        Address = c.String(maxLength: 150),
                        DescriptionId = c.Long(nullable: false),
                        IsWarrantyAvailable = c.Boolean(nullable: false),
                        IsWarrantyPaperEnclosed = c.Boolean(nullable: false),
                        StateStatus = c.String(maxLength: 20),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JodOrderId);
            
            CreateTable(
                "dbo.tblJobOrderProblems",
                c => new
                    {
                        JobOrderProblemId = c.Long(nullable: false, identity: true),
                        ProblemId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        JobOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderProblemId)
                .ForeignKey("dbo.tblJobOrders", t => t.JobOrderId, cascadeDelete: true)
                .Index(t => t.JobOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblJobOrderAccessories", "JobOrderId", "dbo.tblJobOrders");
            DropForeignKey("dbo.tblJobOrderProblems", "JobOrderId", "dbo.tblJobOrders");
            DropIndex("dbo.tblJobOrderProblems", new[] { "JobOrderId" });
            DropIndex("dbo.tblJobOrderAccessories", new[] { "JobOrderId" });
            DropTable("dbo.tblJobOrderProblems");
            DropTable("dbo.tblJobOrders");
            DropTable("dbo.tblJobOrderAccessories");
        }
    }
}
