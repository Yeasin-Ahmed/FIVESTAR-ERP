namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrderTS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblJobOrderTS",
                c => new
                    {
                        JTSId = c.Long(nullable: false, identity: true),
                        JobOrderCode = c.String(maxLength: 100),
                        TSId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                        StateStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 200),
                        AssignDate = c.DateTime(),
                        SignOutDate = c.DateTime(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        JodOrderId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.JTSId)
                .ForeignKey("dbo.tblJobOrders", t => t.JodOrderId, cascadeDelete: true)
                .Index(t => t.JodOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblJobOrderTS", "JodOrderId", "dbo.tblJobOrders");
            DropIndex("dbo.tblJobOrderTS", new[] { "JodOrderId" });
            DropTable("dbo.tblJobOrderTS");
        }
    }
}
