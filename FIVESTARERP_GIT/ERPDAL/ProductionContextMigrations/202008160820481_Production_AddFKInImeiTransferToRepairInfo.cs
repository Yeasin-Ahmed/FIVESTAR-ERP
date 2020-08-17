namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddFKInImeiTransferToRepairInfo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tblIMEITransferToRepairInfo", "TransferId");
            AddForeignKey("dbo.tblIMEITransferToRepairInfo", "TransferId", "dbo.tblTransferToPackagingRepairInfo", "TPRInfoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblIMEITransferToRepairInfo", "TransferId", "dbo.tblTransferToPackagingRepairInfo");
            DropIndex("dbo.tblIMEITransferToRepairInfo", new[] { "TransferId" });
        }
    }
}
