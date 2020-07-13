namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddRequistionFor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionDetails", "IssueQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblRequsitionInfo", "RequisitionFor", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfo", "RequisitionFor");
            DropColumn("dbo.tblRequsitionDetails", "IssueQty");
        }
    }
}
