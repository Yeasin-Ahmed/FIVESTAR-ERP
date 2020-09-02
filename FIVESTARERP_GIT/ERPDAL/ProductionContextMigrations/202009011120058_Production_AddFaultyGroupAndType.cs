namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddFaultyGroupAndType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyCase", "FaultyGroup", c => c.String(maxLength: 50));
            AddColumn("dbo.tblFaultyCase", "FaultyType", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFaultyCase", "FaultyType");
            DropColumn("dbo.tblFaultyCase", "FaultyGroup");
        }
    }
}
