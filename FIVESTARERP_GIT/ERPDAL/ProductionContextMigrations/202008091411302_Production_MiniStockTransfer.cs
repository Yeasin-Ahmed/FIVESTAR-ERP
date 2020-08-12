namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_MiniStockTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMiniStockTransferDetail",
                c => new
                    {
                        MSTDetailId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        MSTInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.MSTDetailId)
                .ForeignKey("dbo.tblMiniStockTransferInfo", t => t.MSTInfoId, cascadeDelete: true)
                .Index(t => t.MSTInfoId);
            
            CreateTable(
                "dbo.tblMiniStockTransferInfo",
                c => new
                    {
                        MSTInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        FloorId = c.Long(nullable: false),
                        PackagingLineId = c.Long(nullable: false),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MSTInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblMiniStockTransferDetail", "MSTInfoId", "dbo.tblMiniStockTransferInfo");
            DropIndex("dbo.tblMiniStockTransferDetail", new[] { "MSTInfoId" });
            DropTable("dbo.tblMiniStockTransferInfo");
            DropTable("dbo.tblMiniStockTransferDetail");
        }
    }
}
