namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_FaultyCase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFaultyCase",
                c => new
                    {
                        CaseId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        ProblemDescription = c.String(maxLength: 200),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CaseId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFaultyCase");
        }
    }
}
