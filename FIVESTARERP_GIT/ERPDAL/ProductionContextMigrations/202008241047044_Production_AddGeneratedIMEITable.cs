namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddGeneratedIMEITable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblGeneratedIMEI",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CodeId = c.Long(nullable: false),
                        QRCode = c.String(maxLength: 100),
                        DescriptionId = c.Long(nullable: false),
                        DescriptionName = c.String(),
                        FloorId = c.Long(),
                        ProductionFloorName = c.String(maxLength: 100),
                        AssemblyLineId = c.Long(),
                        AssemblyLineName = c.String(maxLength: 100),
                        PackagingLineId = c.Long(),
                        PackagingLineName = c.String(maxLength: 100),
                        IMEI = c.String(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblGeneratedIMEI");
        }
    }
}
