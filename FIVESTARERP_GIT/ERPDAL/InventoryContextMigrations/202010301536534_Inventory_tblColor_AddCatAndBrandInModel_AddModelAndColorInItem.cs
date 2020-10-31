namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_tblColor_AddCatAndBrandInModel_AddModelAndColorInItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblColors",
                c => new
                    {
                        ColorId = c.Long(nullable: false, identity: true),
                        ColorName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ColorId);
            
            AddColumn("dbo.tblDescriptions", "CategoryId", c => c.Long());
            AddColumn("dbo.tblDescriptions", "BrandId", c => c.Long());
            AddColumn("dbo.tblDescriptions", "ColorId", c => c.String());
            AddColumn("dbo.tblItems", "DescriptionId", c => c.Long());
            AddColumn("dbo.tblItems", "ColorId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItems", "ColorId");
            DropColumn("dbo.tblItems", "DescriptionId");
            DropColumn("dbo.tblDescriptions", "ColorId");
            DropColumn("dbo.tblDescriptions", "BrandId");
            DropColumn("dbo.tblDescriptions", "CategoryId");
            DropTable("dbo.tblColors");
        }
    }
}
