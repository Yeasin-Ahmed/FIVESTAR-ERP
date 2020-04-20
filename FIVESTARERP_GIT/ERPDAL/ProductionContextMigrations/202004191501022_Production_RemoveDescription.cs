namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_RemoveDescription : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.tblDescriptions");
        }
        
        public override void Down()
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
    }
}
