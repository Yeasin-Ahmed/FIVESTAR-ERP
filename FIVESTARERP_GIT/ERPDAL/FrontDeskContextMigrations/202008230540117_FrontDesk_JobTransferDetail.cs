namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobTransferDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblJobOrderTransferDetail",
                c => new
                    {
                        JobOrderTransferDetailId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        JobOrderId = c.Long(nullable: false),
                        JobOrderCode = c.String(),
                        JobStatus = c.String(),
                        TransferStatus = c.String(),
                        FromBranch = c.Long(),
                        ToBranch = c.Long(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobOrderTransferDetailId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblJobOrderTransferDetail");
        }
    }
}
