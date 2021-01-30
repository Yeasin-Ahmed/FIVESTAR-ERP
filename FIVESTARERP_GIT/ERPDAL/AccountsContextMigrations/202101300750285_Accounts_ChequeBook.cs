namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_ChequeBook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblChequeBooks",
                c => new
                    {
                        ChequeBookId = c.Long(nullable: false, identity: true),
                        AccName = c.String(),
                        AccountNumber = c.String(),
                        BankName = c.String(),
                        BranchName = c.String(),
                        CheckNo = c.String(),
                        CheckDate = c.DateTime(),
                        CheckType = c.String(),
                        PayType = c.String(),
                        CheckApproval = c.String(),
                        Amount = c.Double(nullable: false),
                        PayOrReceive = c.String(),
                        Flag = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ChequeBookId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblChequeBooks");
        }
    }
}
