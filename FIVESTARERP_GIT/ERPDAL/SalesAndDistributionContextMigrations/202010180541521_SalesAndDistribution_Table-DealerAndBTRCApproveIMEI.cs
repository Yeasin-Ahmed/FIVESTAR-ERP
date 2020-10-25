namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_TableDealerAndBTRCApproveIMEI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBTRCApprovedIMEI",
                c => new
                    {
                        IMEIId = c.Long(nullable: false, identity: true),
                        DescriptionId = c.Long(),
                        IMEI = c.String(maxLength: 200),
                        StateStatus = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IMEIId);
            
            CreateTable(
                "dbo.tblDealer",
                c => new
                    {
                        DealerId = c.Long(nullable: false, identity: true),
                        DealerName = c.String(maxLength: 200),
                        Address = c.String(maxLength: 300),
                        TelephoneNo = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 100),
                        Email = c.String(maxLength: 200),
                        ContactPersonName = c.String(maxLength: 150),
                        ContactPersonMobile = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DealerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblDealer");
            DropTable("dbo.tblBTRCApprovedIMEI");
        }
    }
}
