namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ItemReturnInfo_DescriptionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemReturnInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemReturnInfo", "DescriptionId");
        }
    }
}
