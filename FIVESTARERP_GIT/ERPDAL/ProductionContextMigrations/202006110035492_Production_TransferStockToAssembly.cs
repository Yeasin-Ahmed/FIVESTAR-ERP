namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TransferStockToAssembly : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferStockToAssemblyDetail",
                c => new
                    {
                        TSADetailId = c.Long(nullable: false, identity: true),
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
                        TSAInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TSADetailId)
                .ForeignKey("dbo.tblTransferStockToAssemblyInfo", t => t.TSAInfoId, cascadeDelete: true)
                .Index(t => t.TSAInfoId);
            
            CreateTable(
                "dbo.tblTransferStockToAssemblyInfo",
                c => new
                    {
                        TSAInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        AssemblyId = c.Long(),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TSAInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferStockToAssemblyDetail", "TSAInfoId", "dbo.tblTransferStockToAssemblyInfo");
            DropIndex("dbo.tblTransferStockToAssemblyDetail", new[] { "TSAInfoId" });
            DropTable("dbo.tblTransferStockToAssemblyInfo");
            DropTable("dbo.tblTransferStockToAssemblyDetail");
        }
    }
}
