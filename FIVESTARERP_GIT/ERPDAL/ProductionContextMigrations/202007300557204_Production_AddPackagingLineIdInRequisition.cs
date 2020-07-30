namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingLineIdInRequisition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionInfo", "PackagingLineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfo", "PackagingLineId");
        }
    }
}
