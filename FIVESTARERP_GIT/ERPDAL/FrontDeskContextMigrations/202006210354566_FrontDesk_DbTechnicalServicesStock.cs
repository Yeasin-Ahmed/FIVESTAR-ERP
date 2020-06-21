namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbTechnicalServicesStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTechnicalServicesStock",
                c => new
                    {
                        TsStockId = c.Long(nullable: false, identity: true),
                        JobOrderId = c.Long(),
                        SWarehouseId = c.Long(),
                        PartsId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        UsedQty = c.Int(nullable: false),
                        ReturnQty = c.Int(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TsStockId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblTechnicalServicesStock");
        }
    }
}
