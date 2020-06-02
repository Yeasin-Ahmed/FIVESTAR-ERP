namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_Changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblClientProblems",
                c => new
                    {
                        ProblemId = c.Long(nullable: false, identity: true),
                        ProblemName = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProblemId);
            
            CreateTable(
                "dbo.tblMobileParts",
                c => new
                    {
                        MobilePartId = c.Long(nullable: false, identity: true),
                        MobilePartName = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MobilePartId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblMobileParts");
            DropTable("dbo.tblClientProblems");
        }
    }
}
