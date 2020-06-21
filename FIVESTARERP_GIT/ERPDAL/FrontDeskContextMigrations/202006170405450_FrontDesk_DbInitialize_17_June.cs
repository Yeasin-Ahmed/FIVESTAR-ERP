namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbInitialize_17_June : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRequsitionDetailForJobOrders",
                c => new
                    {
                        RequsitionDetailForJobOrderId = c.Long(nullable: false, identity: true),
                        PartsId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        JobOrderId = c.Long(),
                        JobOrderCode = c.String(),
                        Remarks = c.String(),
                        SWarehouseId = c.Long(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RequsitionDetailForJobOrderId);
            
            CreateTable(
                "dbo.tblRequsitionInfoForJobOrders",
                c => new
                    {
                        RequsitionInfoForJobOrderId = c.Long(nullable: false, identity: true),
                        RequsitionCode = c.String(),
                        SWarehouseId = c.Long(),
                        StateStatus = c.String(),
                        JobOrderId = c.Long(),
                        JobOrderCode = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RequsitionInfoForJobOrderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRequsitionInfoForJobOrders");
            DropTable("dbo.tblRequsitionDetailForJobOrders");
        }
    }
}
