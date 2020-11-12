namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_tbl_SalesRepresentative : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSalesRepresentatives",
                c => new
                    {
                        SRID = c.Long(nullable: false, identity: true),
                        FullName = c.String(maxLength: 150),
                        SRType = c.String(maxLength: 100),
                        DivisionId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        ReportingSRId = c.Long(),
                        IsAllowToLogIn = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        BranchId = c.Long(nullable: false),
                        MobileNo = c.String(maxLength: 50),
                        Address = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        UserId = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SRID);
            
            AlterColumn("dbo.tblZone", "ZoneName", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblZone", "ZoneName", c => c.String(maxLength: 150));
            DropTable("dbo.tblSalesRepresentatives");
        }
    }
}
