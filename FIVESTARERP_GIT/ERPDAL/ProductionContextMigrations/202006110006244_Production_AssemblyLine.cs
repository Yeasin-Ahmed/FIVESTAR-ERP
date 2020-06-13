namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AssemblyLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAssemblyLines",
                c => new
                    {
                        AssemblyLineId = c.Long(nullable: false, identity: true),
                        AssemblyLineName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ProductionLineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.AssemblyLineId)
                .ForeignKey("dbo.tblProductionLines", t => t.ProductionLineId, cascadeDelete: true)
                .Index(t => t.ProductionLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblAssemblyLines", "ProductionLineId", "dbo.tblProductionLines");
            DropIndex("dbo.tblAssemblyLines", new[] { "ProductionLineId" });
            DropTable("dbo.tblAssemblyLines");
        }
    }
}
