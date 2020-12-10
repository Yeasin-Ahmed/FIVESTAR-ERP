namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_add_tblFaultyStockAssignTS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFaultyStockAssignTS",
                c => new
                    {
                        FaultyStockAssignTSId = c.Long(nullable: false, identity: true),
                        FaultyStockInfoId = c.Long(),
                        DescriptionId = c.Long(),
                        PartsId = c.Long(),
                        TSId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FaultyStockAssignTSId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFaultyStockAssignTS");
        }
    }
}
