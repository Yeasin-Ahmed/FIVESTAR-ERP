namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QualityControl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQualityControl",
                c => new
                    {
                        QCId = c.Long(nullable: false, identity: true),
                        QCName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QCId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblQualityControl", "ProductionLineId", "dbo.tblProductionLines");
            DropIndex("dbo.tblQualityControl", new[] { "ProductionLineId" });
            DropTable("dbo.tblQualityControl");
        }
    }
}
