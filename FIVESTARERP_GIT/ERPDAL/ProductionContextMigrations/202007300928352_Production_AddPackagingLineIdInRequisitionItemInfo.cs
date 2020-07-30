namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddPackagingLineIdInRequisitionItemInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequisitionItemInfo", "PackagingLineId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequisitionItemInfo", "PackagingLineId");
        }
    }
}
