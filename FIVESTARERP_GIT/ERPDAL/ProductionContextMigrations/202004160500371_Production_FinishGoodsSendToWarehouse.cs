namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FinishGoodsSendToWarehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFinishGoodsSendToWarehouseDetail",
                c => new
                    {
                        SendDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        Flag = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        SendId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SendDetailId)
                .ForeignKey("dbo.tblFinishGoodsSendToWarehouseInfo", t => t.SendId, cascadeDelete: true)
                .Index(t => t.SendId);
            
            CreateTable(
                "dbo.tblFinishGoodsSendToWarehouseInfo",
                c => new
                    {
                        SendId = c.Long(nullable: false, identity: true),
                        LineId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        Flag = c.String(maxLength: 150),
                        StateStatus = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblFinishGoodsSendToWarehouseDetail", "SendId", "dbo.tblFinishGoodsSendToWarehouseInfo");
            DropIndex("dbo.tblFinishGoodsSendToWarehouseDetail", new[] { "SendId" });
            DropTable("dbo.tblFinishGoodsSendToWarehouseInfo");
            DropTable("dbo.tblFinishGoodsSendToWarehouseDetail");
        }
    }
}
