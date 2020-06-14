namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbTransferInfoDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferDetails",
                c => new
                    {
                        TransferDetailId = c.Long(nullable: false, identity: true),
                        PartsId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TransferInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TransferDetailId)
                .ForeignKey("dbo.tblTransferInfo", t => t.TransferInfoId, cascadeDelete: true)
                .Index(t => t.TransferInfoId);
            
            CreateTable(
                "dbo.tblTransferInfo",
                c => new
                    {
                        TransferInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        BranchTo = c.Long(),
                        WarehouseId = c.Long(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransferInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferDetails", "TransferInfoId", "dbo.tblTransferInfo");
            DropIndex("dbo.tblTransferDetails", new[] { "TransferInfoId" });
            DropTable("dbo.tblTransferInfo");
            DropTable("dbo.tblTransferDetails");
        }
    }
}
