namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddRequisitionItemDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRequisitionItemDetail",
                c => new
                    {
                        ReqItemDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemId = c.Long(),
                        ItemTypeId = c.Long(),
                        ConsumptionQty = c.Int(),
                        TotalQuantity = c.Int(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ReqItemInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqItemDetailId)
                .ForeignKey("dbo.tblRequisitionItemInfo", t => t.ReqItemInfoId, cascadeDelete: true)
                .Index(t => t.ReqItemInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequisitionItemDetail", "ReqItemInfoId", "dbo.tblRequisitionItemInfo");
            DropIndex("dbo.tblRequisitionItemDetail", new[] { "ReqItemInfoId" });
            DropTable("dbo.tblRequisitionItemDetail");
        }
    }
}
