namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_JournalDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblJournal", "JournalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblJournal", "JournalDate", c => c.String());
        }
    }
}
