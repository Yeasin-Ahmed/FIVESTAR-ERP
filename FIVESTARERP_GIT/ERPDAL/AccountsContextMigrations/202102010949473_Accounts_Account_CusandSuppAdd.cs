namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_Account_CusandSuppAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblAccount", "CustomerId", c => c.Long());
            AddColumn("dbo.tblAccount", "SupplierId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblAccount", "SupplierId");
            DropColumn("dbo.tblAccount", "CustomerId");
        }
    }
}
