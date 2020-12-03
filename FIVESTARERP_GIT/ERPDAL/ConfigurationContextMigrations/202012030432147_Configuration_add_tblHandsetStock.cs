namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_tblHandsetStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblHandSetStock",
                c => new
                    {
                        HandSetStockId = c.Long(nullable: false, identity: true),
                        IMEI = c.String(),
                        DescriptionId = c.Long(nullable: false),
                        ColorId = c.Long(nullable: false),
                        StockType = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        Remarks = c.String(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HandSetStockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblHandSetStock");
        }
    }
}
