namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferStockToQC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferStockToQCDetail",
                c => new
                    {
                        TSQDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        TSQInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TSQDetailId)
                .ForeignKey("dbo.tblTransferStockToQCInfo", t => t.TSQInfoId, cascadeDelete: true)
                .Index(t => t.TSQInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToQCInfo",
                c => new
                    {
                        TSQInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        AssemblyId = c.Long(),
                        QCLineId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TSQInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferStockToQCDetail", "TSQInfoId", "dbo.tblTransferStockToQCInfo");
            DropIndex("dbo.tblTransferStockToQCDetail", new[] { "TSQInfoId" });
            DropTable("dbo.tblTransferStockToQCInfo");
            DropTable("dbo.tblTransferStockToQCDetail");
        }
    }
}
