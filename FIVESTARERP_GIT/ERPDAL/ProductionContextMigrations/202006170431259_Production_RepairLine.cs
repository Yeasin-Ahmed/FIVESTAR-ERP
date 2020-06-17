namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RepairLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairLine",
                c => new
                    {
                        RepairLineId = c.Long(nullable: false, identity: true),
                        RepairLineName = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RepairLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairLine", "ProductionLineId", "dbo.tblProductionLines");
            DropIndex("dbo.tblRepairLine", new[] { "ProductionLineId" });
            DropTable("dbo.tblRepairLine");
        }
    }
}
