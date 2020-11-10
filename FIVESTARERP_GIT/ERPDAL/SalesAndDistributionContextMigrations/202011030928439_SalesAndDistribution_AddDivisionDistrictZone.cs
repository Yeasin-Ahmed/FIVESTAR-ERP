namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddDivisionDistrictZone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDistrict",
                c => new
                    {
                        DistrictId = c.Long(nullable: false, identity: true),
                        DistrictName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        DivisionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DistrictId)
                .ForeignKey("dbo.tblDivision", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.tblDivision",
                c => new
                    {
                        DivisionId = c.Long(nullable: false, identity: true),
                        DivisionName = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        CountryId = c.Long(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DivisionId);
            
            CreateTable(
                "dbo.tblZone",
                c => new
                    {
                        ZoneId = c.Long(nullable: false, identity: true),
                        ZoneName = c.String(maxLength: 150),
                        PostalCode = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 200),
                        BranchId = c.Long(),
                        DivisionId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        DistrictId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneId)
                .ForeignKey("dbo.tblDistrict", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblZone", "DistrictId", "dbo.tblDistrict");
            DropForeignKey("dbo.tblDistrict", "DivisionId", "dbo.tblDivision");
            DropIndex("dbo.tblZone", new[] { "DistrictId" });
            DropIndex("dbo.tblDistrict", new[] { "DivisionId" });
            DropTable("dbo.tblZone");
            DropTable("dbo.tblDivision");
            DropTable("dbo.tblDistrict");
        }
    }
}
