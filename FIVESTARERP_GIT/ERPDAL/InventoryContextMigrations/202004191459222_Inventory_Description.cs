namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_Description : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDescriptions",
                c => new
                    {
                        DescriptionId = c.Long(nullable: false, identity: true),
                        DescriptionName = c.String(),
                        SubCategoryId = c.Long(),
                        Remarks = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DescriptionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblDescriptions");
        }
    }
}
