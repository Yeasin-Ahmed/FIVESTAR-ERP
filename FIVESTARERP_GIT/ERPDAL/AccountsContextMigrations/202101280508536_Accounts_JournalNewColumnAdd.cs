namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_JournalNewColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJournal", "PersonalCode", c => c.String());
            AddColumn("dbo.tblJournal", "VoucherNo", c => c.String());
            AddColumn("dbo.tblJournal", "DueAmount", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJournal", "DueAmount");
            DropColumn("dbo.tblJournal", "VoucherNo");
            DropColumn("dbo.tblJournal", "PersonalCode");
        }
    }
}
