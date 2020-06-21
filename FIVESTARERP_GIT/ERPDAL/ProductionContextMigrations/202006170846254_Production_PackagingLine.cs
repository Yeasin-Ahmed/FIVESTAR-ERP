namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_PackagingLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblPackagingLine",
                c => new
                    {
                        PackagingLineId = c.Long(nullable: false, identity: true),
                        PackagingLineName = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PackagingLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblPackagingLine", "ProductionLineId", "dbo.tblProductionLines");
            DropIndex("dbo.tblPackagingLine", new[] { "ProductionLineId" });
            DropTable("dbo.tblPackagingLine");
        }
    }
}
