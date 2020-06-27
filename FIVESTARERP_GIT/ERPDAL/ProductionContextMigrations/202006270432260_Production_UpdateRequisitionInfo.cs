namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_UpdateRequisitionInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQRCodeTrace",
                c => new
                    {
                        CodeId = c.Long(nullable: false, identity: true),
                        CodeNo = c.String(maxLength: 200),
                        ProductionFloorId = c.Long(),
                        DescriptionId = c.Long(),
                        ColorId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ColorName = c.String(maxLength: 150),
                        ReferenceNumber = c.String(maxLength: 200),
                        ReferenceId = c.String(),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CodeId);
            
            AddColumn("dbo.tblRequsitionInfo", "IsBundle", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblRequsitionInfo", "ItemTypeId", c => c.Long());
            AddColumn("dbo.tblRequsitionInfo", "ItemId", c => c.Long());
            AddColumn("dbo.tblRequsitionInfo", "ForQTy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfo", "ForQTy");
            DropColumn("dbo.tblRequsitionInfo", "ItemId");
            DropColumn("dbo.tblRequsitionInfo", "ItemTypeId");
            DropColumn("dbo.tblRequsitionInfo", "IsBundle");
            DropTable("dbo.tblQRCodeTrace");
        }
    }
}
