namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_tblFinanceYear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFinanceYear",
                c => new
                    {
                        FinanceYearId = c.Long(nullable: false, identity: true),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        Session = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FinanceYearId);
            
            DropTable("dbo.tblFinance");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblFinance",
                c => new
                    {
                        FinanceId = c.Long(nullable: false, identity: true),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        Session = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FinanceId);
            
            DropTable("dbo.tblFinanceYear");
        }
    }
}
