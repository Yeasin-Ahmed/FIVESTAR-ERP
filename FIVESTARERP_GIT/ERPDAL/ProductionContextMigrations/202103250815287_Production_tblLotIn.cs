namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_tblLotIn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblLotInLog",
                c => new
                    {
                        LotInLogId = c.Long(nullable: false, identity: true),
                        CodeId = c.Long(),
                        CodeNo = c.String(maxLength: 200),
                        ProductionFloorId = c.Long(),
                        AssemblyId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(maxLength: 100),
                        ReferenceNumber = c.String(maxLength: 200),
                        ReferenceId = c.String(),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LotInLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblLotInLog");
        }
    }
}
