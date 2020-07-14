namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_TsStockInfoDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTsStockReturnDetails",
                c => new
                    {
                        ReturnDetailId = c.Long(nullable: false, identity: true),
                        ReqDetailId = c.Long(nullable: false),
                        JobOrderId = c.Long(nullable: false),
                        RequsitionCode = c.String(),
                        PartsId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        ReturnInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReturnDetailId)
                .ForeignKey("dbo.tblTsStockReturnInfo", t => t.ReturnInfoId, cascadeDelete: true)
                .Index(t => t.ReturnInfoId);
            
            CreateTable(
                "dbo.tblTsStockReturnInfo",
                c => new
                    {
                        ReturnInfoId = c.Long(nullable: false, identity: true),
                        ReqInfoId = c.Long(nullable: false),
                        JobOrderId = c.Long(nullable: false),
                        RequsitionCode = c.String(),
                        StateStatus = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReturnInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTsStockReturnDetails", "ReturnInfoId", "dbo.tblTsStockReturnInfo");
            DropIndex("dbo.tblTsStockReturnDetails", new[] { "ReturnInfoId" });
            DropTable("dbo.tblTsStockReturnInfo");
            DropTable("dbo.tblTsStockReturnDetails");
        }
    }
}
