namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_tblAccountsHead : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAccountsHead",
                c => new
                    {
                        AHeadId = c.Long(nullable: false, identity: true),
                        AHeadName = c.String(),
                        AHeadCode = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AHeadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblAccountsHead");
        }
    }
}
