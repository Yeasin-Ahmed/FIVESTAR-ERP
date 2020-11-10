namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_IsHandset_Columnadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "IsHandset", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "IsHandset");
        }
    }
}
