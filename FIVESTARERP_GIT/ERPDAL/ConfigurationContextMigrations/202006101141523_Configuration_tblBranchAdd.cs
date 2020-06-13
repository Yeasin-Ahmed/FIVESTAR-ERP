namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_tblBranchAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBranches",
                c => new
                    {
                        BranchId = c.Long(nullable: false, identity: true),
                        BranchName = c.String(),
                        BranchAddress = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblBranches");
        }
    }
}
