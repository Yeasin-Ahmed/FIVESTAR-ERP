namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbtblTS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTechnicalServiceEngs",
                c => new
                    {
                        EngId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblTechnicalServiceEngs");
        }
    }
}
