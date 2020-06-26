namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_QCItemQCId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQCItemStockDetail", "QCId", c => c.Long());
            AddColumn("dbo.tblQCItemStockInfo", "QCId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQCItemStockInfo", "QCId");
            DropColumn("dbo.tblQCItemStockDetail", "QCId");
        }
    }
}
