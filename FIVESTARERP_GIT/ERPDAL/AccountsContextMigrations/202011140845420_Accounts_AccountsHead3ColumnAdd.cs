namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_AccountsHead3ColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblAccountsHead", "IsGroupHead", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblAccountsHead", "AccountType", c => c.String());
            AddColumn("dbo.tblAccountsHead", "AncestorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblAccountsHead", "AncestorId");
            DropColumn("dbo.tblAccountsHead", "AccountType");
            DropColumn("dbo.tblAccountsHead", "IsGroupHead");
        }
    }
}
