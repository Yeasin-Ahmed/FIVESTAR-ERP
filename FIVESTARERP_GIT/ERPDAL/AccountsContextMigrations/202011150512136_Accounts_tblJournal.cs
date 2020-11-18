namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_tblJournal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAccount",
                c => new
                    {
                        AccountId = c.Long(nullable: false, identity: true),
                        AccountName = c.String(),
                        AccountCode = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        IsGroupHead = c.Boolean(nullable: false),
                        AccountType = c.String(),
                        AncestorId = c.String(),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.tblJournal",
                c => new
                    {
                        JournalId = c.Long(nullable: false, identity: true),
                        ReferenceNum = c.String(),
                        AccountId = c.Long(nullable: false),
                        Debit = c.Double(nullable: false),
                        Credit = c.Double(nullable: false),
                        Flag = c.String(),
                        Remarks = c.String(),
                        Narration = c.String(),
                        Year = c.Short(nullable: false),
                        Month = c.Short(nullable: false),
                        Day = c.Short(nullable: false),
                        JournalDate = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ApprovedBy = c.Long(),
                        ApproveDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JournalId);
            
            DropTable("dbo.tblAccountsHead");
        }
        
        public override void Down()
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
                        IsGroupHead = c.Boolean(nullable: false),
                        AccountType = c.String(),
                        AncestorId = c.String(),
                    })
                .PrimaryKey(t => t.AHeadId);
            
            DropTable("dbo.tblJournal");
            DropTable("dbo.tblAccount");
        }
    }
}
