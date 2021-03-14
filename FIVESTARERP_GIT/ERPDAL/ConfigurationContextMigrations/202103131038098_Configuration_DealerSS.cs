namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DealerSS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDealerSS",
                c => new
                    {
                        DealerId = c.Long(nullable: false, identity: true),
                        DealerName = c.String(),
                        DealerCode = c.String(),
                        Address = c.String(),
                        TelephoneNo = c.String(),
                        MobileNo = c.String(),
                        Email = c.String(),
                        DivisionName = c.String(),
                        DistrictName = c.String(),
                        ZoneName = c.String(),
                        ContactPersonName = c.String(),
                        ContactPersonMobile = c.String(),
                        Remarks = c.String(),
                        Flag = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DealerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblDealerSS");
        }
    }
}
