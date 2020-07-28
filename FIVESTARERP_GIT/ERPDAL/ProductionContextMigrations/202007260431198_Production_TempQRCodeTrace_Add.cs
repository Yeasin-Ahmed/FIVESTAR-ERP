namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_TempQRCodeTrace_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTempQRCodeTrace",
                c => new
                    {
                        CodeId = c.Long(nullable: false, identity: true),
                        CodeNo = c.String(maxLength: 200),
                        ProductionFloorId = c.Long(),
                        AssemblyId = c.Long(),
                        ProductionFloorName = c.String(),
                        AssemblyLineName = c.String(),
                        DescriptionId = c.Long(),
                        ModelName = c.String(),
                        ColorId = c.Long(),
                        ColorName = c.String(maxLength: 150),
                        WarehouseId = c.Long(),
                        WarehouseName = c.String(),
                        ItemTypeId = c.Long(),
                        ItemTypeName = c.String(),
                        ItemId = c.Long(),
                        ItemName = c.String(),
                        ReferenceNumber = c.String(maxLength: 200),
                        ReferenceId = c.String(),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        IMEI = c.String(maxLength: 300),
                        BatteryCode = c.String(maxLength: 200),
                        StateStatus = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblTempQRCodeTrace");
        }
    }
}
