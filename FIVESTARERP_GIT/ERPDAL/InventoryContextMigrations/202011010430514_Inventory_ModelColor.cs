namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_ModelColor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblModelColors",
                c => new
                    {
                        DescriptionId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.DescriptionId, t.ColorId })
                .ForeignKey("dbo.tblColors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.tblDescriptions", t => t.DescriptionId, cascadeDelete: true)
                .Index(t => t.DescriptionId)
                .Index(t => t.ColorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblModelColors", "DescriptionId", "dbo.tblDescriptions");
            DropForeignKey("dbo.tblModelColors", "ColorId", "dbo.tblColors");
            DropIndex("dbo.tblModelColors", new[] { "ColorId" });
            DropIndex("dbo.tblModelColors", new[] { "DescriptionId" });
            DropTable("dbo.tblModelColors");
        }
    }
}
