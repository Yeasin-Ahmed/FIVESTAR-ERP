namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AssemblyLineInQRCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeTrace", "AssemblyId", c => c.Long());
            AddColumn("dbo.tblQRCodeTrace", "AssemblyLineName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQRCodeTrace", "AssemblyLineName");
            DropColumn("dbo.tblQRCodeTrace", "AssemblyId");
        }
    }
}
