namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemPreparation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblItemPreparationDetail",
                c => new
                    {
                        PreparationDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        PreparationInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PreparationDetailId)
                .ForeignKey("dbo.tblItemPreparationInfo", t => t.PreparationInfoId, cascadeDelete: true)
                .Index(t => t.PreparationInfoId);
            
            CreateTable(
                "dbo.tblItemPreparationInfo",
                c => new
                    {
                        PreparationInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PreparationInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblItemPreparationDetail", "PreparationInfoId", "dbo.tblItemPreparationInfo");
            DropIndex("dbo.tblItemPreparationDetail", new[] { "PreparationInfoId" });
            DropTable("dbo.tblItemPreparationInfo");
            DropTable("dbo.tblItemPreparationDetail");
        }
    }
}
