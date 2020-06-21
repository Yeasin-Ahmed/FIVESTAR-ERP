namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_DbInitialize_17_June_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionDetailForJobOrders", "RequsitionInfoForJobOrderId", c => c.Long(nullable: false));
            CreateIndex("dbo.tblRequsitionDetailForJobOrders", "RequsitionInfoForJobOrderId");
            AddForeignKey("dbo.tblRequsitionDetailForJobOrders", "RequsitionInfoForJobOrderId", "dbo.tblRequsitionInfoForJobOrders", "RequsitionInfoForJobOrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequsitionDetailForJobOrders", "RequsitionInfoForJobOrderId", "dbo.tblRequsitionInfoForJobOrders");
            DropIndex("dbo.tblRequsitionDetailForJobOrders", new[] { "RequsitionInfoForJobOrderId" });
            DropColumn("dbo.tblRequsitionDetailForJobOrders", "RequsitionInfoForJobOrderId");
        }
    }
}
