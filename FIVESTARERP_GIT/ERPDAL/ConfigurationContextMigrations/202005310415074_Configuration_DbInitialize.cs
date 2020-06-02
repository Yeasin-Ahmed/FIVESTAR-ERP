namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbInitialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAccessories",
                c => new
                    {
                        AccessoriesId = c.Long(nullable: false, identity: true),
                        AccessoriesName = c.String(),
                        Remarks = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccessoriesId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblAccessories");
        }
    }
}
