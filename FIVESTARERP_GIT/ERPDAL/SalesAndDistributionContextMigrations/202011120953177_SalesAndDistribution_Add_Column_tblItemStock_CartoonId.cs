namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_Add_Column_tblItemStock_CartoonId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemStock", "CartoonId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemStock", "CartoonId");
        }
    }
}
