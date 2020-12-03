namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_tblMissingStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMissingStock",
                c => new
                    {
                        MissingStockId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        StockType = c.String(),
                        Quantity = c.Int(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        Remarks = c.String(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MissingStockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblMissingStock");
        }
    }
}
