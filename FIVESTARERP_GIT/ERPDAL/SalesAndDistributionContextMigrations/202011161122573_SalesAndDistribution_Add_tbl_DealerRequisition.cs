namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_Add_tbl_DealerRequisition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDealerRequisitionDetail",
                c => new
                    {
                        DREQDetailId = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(nullable: false),
                        BrandId = c.Long(nullable: false),
                        ModelId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IssueQuantity = c.Int(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ApprovedBy = c.Long(),
                        ApprovedDate = c.DateTime(),
                        DREQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DREQDetailId)
                .ForeignKey("dbo.tblDealerRequisitionInfo", t => t.DREQInfoId, cascadeDelete: true)
                .Index(t => t.DREQInfoId);
            
            CreateTable(
                "dbo.tblDealerRequisitionInfo",
                c => new
                    {
                        DREQInfoId = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        RequisitionCode = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ApprovedBy = c.Long(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DREQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblDealerRequisitionDetail", "DREQInfoId", "dbo.tblDealerRequisitionInfo");
            DropIndex("dbo.tblDealerRequisitionDetail", new[] { "DREQInfoId" });
            DropTable("dbo.tblDealerRequisitionInfo");
            DropTable("dbo.tblDealerRequisitionDetail");
        }
    }
}
