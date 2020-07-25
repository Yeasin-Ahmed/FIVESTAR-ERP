namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RequisitionItemInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRequisitionItemInfo",
                c => new
                    {
                        ReqItemInfoId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(nullable: false),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ReqInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqItemInfoId)
                .ForeignKey("dbo.tblRequsitionInfo", t => t.ReqInfoId, cascadeDelete: true)
                .Index(t => t.ReqInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequisitionItemInfo", "ReqInfoId", "dbo.tblRequsitionInfo");
            DropIndex("dbo.tblRequisitionItemInfo", new[] { "ReqInfoId" });
            DropTable("dbo.tblRequisitionItemInfo");
        }
    }
}
